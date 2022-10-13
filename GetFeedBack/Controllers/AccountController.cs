using GetFeedBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace GetFeedBack.Controllers
{
    public class AccountController : BaseController
    {
        private readonly FeedbackContext _db;

        private readonly ITwilioRestClient _client;
        public AccountController(FeedbackContext db, ITwilioRestClient client)
        {
            _db = db;
            _client = client;
        }
        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register([Bind] Users user)
        {
            if (_db.Users.FirstOrDefault(x => x.Email == user.Email) == null)
            {

                user.Password = GetMD5(user.Password);
                user.CreateDate = DateTime.Now;
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.error = "Email already exists ";
                return View();
            }

        }
        [HttpGet]
        public IActionResult Login()
        {
            Users users = new Users();
            return View(users);
        }
        [HttpPost]
        public IActionResult Login(Users users)
        {
            var user = _db.Users.Where(x => x.Email == users.Email).FirstOrDefault();
            try
            {
                var password = GetMD5(users.Password);
                if (user != null && user.Password == password)
                {
                    _ = CreateAuthenticationTicketUserLogin(user);
                    return RedirectToAction(nameof(FeedBackController.Index), "FeedBack", new { Id = user.Id });
                }
                else
                {
                    ViewData["Error"] = "Account or password is not true";
                    return View("Login", users);
                }
            }
            catch (Exception ex)
            {
                return View("Login", users);
            }

        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(Users users)
        {
            var query = _db.Users.Where(x => x.Email == users.Email).FirstOrDefault();
            if (query != null)
            {
                query.Email = users.Email;
                query.Password = GetMD5(users.Password);
                _db.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }


        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(SmsMessage model, Users users)
        {
            var query = _db.Users.Where(x => x.Email == users.Email).FirstOrDefault();
            if (query != null)
            {
                Random rnd = new Random();
                query.Email = users.Email;
                int randompass = rnd.Next(99);
                string pass = "admin";
                query.Password = GetMD5(pass + randompass);
                _db.SaveChanges();
                model.Message = "Email:" + " " + query.Email + " " + "Mật khẩu mới của bạn là:" + " " + pass + randompass;
                var message = MessageResource.Create(
                    to: new PhoneNumber("+84363507303"),
                    from: new PhoneNumber("+17087753229"),
                    body: model.Message,
                    client: _client); // pass in the custom client
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
