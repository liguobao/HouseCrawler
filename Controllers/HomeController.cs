using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Collections.Generic;
using HouseCrawler.Crawlers;

namespace HouseCrawler.Controllers
{
    public class HomeController : Controller
    {
        private LianJiaCrawler lianJiaCrawler;


        public HomeController(LianJiaCrawler lianJiaCrawler)
        {
            this.lianJiaCrawler = lianJiaCrawler;
         }

        public IActionResult Index()
        {
            lianJiaCrawler.Run();
            return Json(new { success = true });
        }

    }
}