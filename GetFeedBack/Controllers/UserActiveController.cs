using GetFeedBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace GetFeedBack.Controllers
{
    public class UserActiveController : Controller
    {
        private readonly FeedbackContext _db;
        public IActionResult Index()
        {
            return View();
        }
    }
}
