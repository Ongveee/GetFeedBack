using GetFeedBack.Logic.Interfaces;
using GetFeedBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
namespace GetFeedBack.Controllers
{
    [Authorize(Role.Admin)]
    public class HomeController : BaseController
    {
        
        private readonly ITestService _testService;
        private readonly IUserService _userService;

        public HomeController(ITestService testService, IUserService userService)
        {
            _testService = testService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            ViewBag.RandomString = _testService.GetRandomString();
            ViewBag.RandomString = _userService.GetRandomString();
            return View();
        }

        public IActionResult Privacy()
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
