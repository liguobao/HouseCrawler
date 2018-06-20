using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace HouseCrawler.Models
{
    public class APPConfiguration
    {

        public string ESURL { get; set; }

        public string ESUserName { get; set; }

        public string ESPassword { get; set; }

        public List<ConfigItem> CityList { get; set; }
    }
}
