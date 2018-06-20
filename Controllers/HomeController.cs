using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Collections.Generic;

namespace HouseCrawler.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Json(new { success = true });
        }

    }
}