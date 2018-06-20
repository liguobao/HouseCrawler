using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Parser.Html;
using HouseCrawler.Models;
using HouseCrawler.Service;
using Microsoft.Extensions.Options;
using RestSharp;

namespace HouseCrawler.Crawlers
{
    public class LianJiaCrawler

    {

        private static HtmlParser htmlParser = new HtmlParser();

        private ElasticsearchService elasticsearchService;

        private APPConfiguration configuration;

        public LianJiaCrawler(ElasticsearchService elasticsearchService, IOptions<APPConfiguration> configuration)
        {
            this.elasticsearchService = elasticsearchService;
            this.configuration = configuration.Value;
        }

        public void Run()
        {
            foreach (var city in configuration.CityList)
            {
                for (var page = 1; page <= 100; page++)
                {
                    var houses = new List<HouseInfo>();
                    var houseUrl = $"https://{city.Code}.lianjia.com/ershoufang/pg{page}/";
                    var houseHTML = GetHTML(houseUrl);
                    var htmlDoc = htmlParser.Parse(houseHTML);
                    var houseUL = htmlDoc.QuerySelector("ul.sellListContent");
                    if (houseUL == null)
                        continue;
                    foreach (var item in houseUL.QuerySelectorAll("li.clear"))
                    {
                        var title = item.QuerySelector("div.title");
                        if (title == null)
                            continue;
                        var house = new HouseInfo();
                        house.HouseTitle = title.QuerySelector("a").TextContent;
                        house.OnlineURL = title.QuerySelector("a").GetAttribute("href");
                        var address = item.QuerySelector("div.houseInfo");
                        if (address != null)
                        {

                            var addressList = address.TextContent.Split("/");
                            if (addressList.Any())
                            {
                                house.Address = address.QuerySelector("a").TextContent;
                                house.HouseType = addressList.FirstOrDefault(text => text.Contains("室"));
                                house.Area = decimal.Parse(addressList.FirstOrDefault(text => text.Contains("平米")).Replace("平米", ""));
                            }
                        }
                        var timeText = item.QuerySelector("div.timeInfo").TextContent;
                        var pubDay = 0;
                        if (timeText.Contains("天"))
                        {
                            pubDay = int.Parse(timeText.Replace("天以前发布", ""));
                        }
                        else if (timeText.Contains("月"))
                        {
                            pubDay = int.Parse(timeText.Replace("个月以前发布", "")) * 30;
                        }
                        house.PubTime = DateTime.Now.AddDays(-pubDay);

                        house.TotalPrice = decimal.Parse(item.QuerySelector("div.totalPrice").QuerySelector("span").TextContent);
                        house.UnitPrice = decimal.Parse(item.QuerySelector("div.unitPrice").GetAttribute("data-price"));
                        house.Pictures = new List<string>() { item.QuerySelector("img.lj-lazy").GetAttribute("src") };
                        house.HouseText = item.InnerHtml;
                        house.CityName = city.Name;
                        houses.Add(house);
                    }
                    elasticsearchService.SaveHousesToES(houses);
                }


            }

        }



        private string GetHTML(string houseUrl)
        {
            var client = new RestClient(houseUrl);
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "c69802a4-d0b0-fa69-cb0e-014b1b28bc0c");
            request.AddHeader("accept-language", "zh-CN,zh;q=0.9,en;q=0.8,da;q=0.7");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("referer", "https://link.zhihu.com/?target=" + houseUrl);
            request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            request.AddHeader("user-agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36");
            request.AddHeader("upgrade-insecure-requests", "1");
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

    }

}
