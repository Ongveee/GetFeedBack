using GetFeedBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace GetFeedBack.Controllers
{
    public class LoginController : BaseController
    {
        FeedbackContext _db;
        public LoginController(FeedbackContext db)
        {
            this._db = db;
        }
        [HttpGet, AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost, AllowAnonymous]
        public IActionResult Index(Admins admins)
        {
            var admin = _db.Admins.Where(x => x.Account.Trim() == admins.Account.Trim() && x.Password == admins.Password).FirstOrDefault();
            try
            {
                if (admin != null)
                {
                    _ = CreateAuthenticationTicket(admin);
                    //Show Success Message -"Welcome!"
                    return RedirectToAction("DashboardStatistics","Admin");
                }
                else
                {
                    //Show Error Message -"Invalid ."
                    return View("Index", admins);
                }
            }
            catch (Exception ex)
            {
                //Show Error Message- ex.Message
                return View("Index", admins);
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
