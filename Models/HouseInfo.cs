using System;
using System.Collections.Generic;
using Nest;

namespace HouseCrawler.Models
{

    [ElasticsearchType(IdProperty = "OnlineURL")]
    public class HouseInfo
    {

        /// <summary>
        /// 标题
        /// </summary>
        public string HouseTitle { get; set; }

        public string HouseText { get; set; }

        /// <summary>
        /// 房间URL
        /// </summary>
        public string OnlineURL { get; set; }

        /// <summary>
        /// 地理位置（一般用于定位）
        /// </summary>
        public string Address { get; set; }

        public string HouseType {get;set;}

        /// <summary>
        /// 价钱（可能非纯数字）
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 价格（纯数字）
        /// </summary>
        public decimal TotalPrice { get; set; }

        public decimal Area { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PubTime { get; set; }


        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime PubDate
        {
            get
            {
                if (this.PubTime == null || this.PubTime == DateTime.MinValue)
                {
                    return DateTime.Now.Date;
                }
                return this.PubTime.Date;
            }
        }


        /// <summary>
        /// 所在城市
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 来源网站
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public List<String> Pictures { get; set; }

    }
}
