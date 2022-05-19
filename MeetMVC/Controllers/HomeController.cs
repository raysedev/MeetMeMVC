using MeetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace MeetMVC.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult Interests()
        {
            var data = new List<InterestsViewModel> {
            new InterestsViewModel { Id=1, Name="Sports"},
            new InterestsViewModel { Id=2, Name="Gaming"},
            new InterestsViewModel { Id=3, Name="Cooking"},
            new InterestsViewModel { Id=4, Name="Reading"},
            new InterestsViewModel { Id=5, Name="Yoga"},
            new InterestsViewModel { Id=6, Name="Art"},
            new InterestsViewModel { Id=6, Name="Music"}
            };
            InterestsViewModel model = new();
             model.ItemList = data.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult PostSelectedValues(PostSelectedViewModel model)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
