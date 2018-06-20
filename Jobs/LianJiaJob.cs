using System.Collections.Generic;
using System.Linq;
using AngleSharp.Parser.Html;
using HouseCrawler.Crawlers;
using HouseCrawler.Models;
using HouseCrawler.Service;
using Pomelo.AspNetCore.TimedJob;
using RestSharp;

namespace HouseCrawler.Jobs
{
    public class LianJiaJob : Job
    {
        private static HtmlParser htmlParser = new HtmlParser();

        private LianJiaCrawler lianJiaCrawler;
        
        public LianJiaJob(LianJiaCrawler lianJiaCrawler)
        {
            this.lianJiaCrawler = lianJiaCrawler;
        }

        [Invoke(Begin = "2018-06-20 00:00", Interval = 1000 * 1800, SkipWhileExecuting = true)]
        public void Run()
        {
           lianJiaCrawler.Run();
        }


    }
}
