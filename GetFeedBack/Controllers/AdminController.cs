using GetFeedBack.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Security.Cryptography;

namespace GetFeedBack.Controllers
{
    [Authorize(Role.Admin)]
    public class AdminController : BaseController
    {
        private readonly FeedbackContext _db;

        public AdminController(FeedbackContext db)
        {
            _db = db;
        }

        public static string GetMD5(string str)
        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            StringBuilder sbHash = new StringBuilder();

            foreach (byte b in bHash)
            {

                sbHash.Append(String.Format("{0:x2}", b));

            }

            return sbHash.ToString();

        }

        public IActionResult DashboardView()
        {
            var data = _db.Users
                .Join(_db.FeedBacks,
                    user => user.Id,
                    feedback => feedback.UserId,
                    (user, feedback) =>
                    new FeedbackUser
                    {
                        Id = feedback.Id,
                        Name = feedback.Name,
                        Username = user.Username,
                        Email = user.Email,
                    }
                ).ToList();
            return View(data);
        }

        public IActionResult DashboardStatistics()
        {

            //Data Monthly
            List<int> userCount = new List<int>();
            List<int> fbCount = new List<int>();
            for (int i = 0; i < 12; ++i)
            {
                var temp = _db.Users.Count(p => p.CreateDate.Month == (i + 1));
                var temp2 = _db.FeedBacks.Count(p => p.CreateDate.Month == (i + 1));
                userCount.Add(temp);
                fbCount.Add(temp2);
            }

            // Data tototototo
            var users = _db.Users.Count();
            var fbs = _db.FeedBacks.Count();
            var fbds = _db.FeedBackDetails.Count();
            int fbboutdate = 0;
            List<Links> links = _db.Links.ToList();
            foreach (var l in links)
            {
                var date = l.Deadline;
                if (DateTime.Compare(DateTime.Now, date) > 0) fbboutdate++;
                else continue;
            }
            // Transfer Data
            ViewBag.userCount = JsonConvert.SerializeObject(userCount);
            ViewBag.fbCount = JsonConvert.SerializeObject(fbCount);
            ViewBag.users = users;
            ViewBag.fbs = fbs;
            ViewBag.fbds = fbds;
            ViewBag.fboutdate = fbboutdate;

            return View();
        }


        public IActionResult UserManagement()
        {
            List<Users> data = _db.Users.ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]

        public IActionResult CreateUser(Users user)
        {
            if (_db.Users.FirstOrDefault(x => x.Email == user.Email) == null)
            {

                user.Password = GetMD5(user.Password);
                user.CreateDate = DateTime.Now;
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("UserManagement");
            }
            else
            {
                ViewBag.error = "Email already exists ";
                return View();
            }
        }

        [HttpGet]

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var user = _db.Users.FirstOrDefault(p => p.Id == id);
            return View(user);

        }

        [HttpPost]

        public IActionResult Edit(Users user)
        {
            if (ModelState.IsValid)
            {
                var us = _db.Users.FirstOrDefault(x => x.Id == user.Id);
                us.Username = user.Username;
                us.Password = user.Password;
                us.Email = user.Email;
                _db.SaveChanges();
            }

            return RedirectToAction("UserManagement");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            Users user = _db.Users.FirstOrDefault(p => p.Id == id);
            //Delete Feedbacks
            while (_db.FeedBacks.Where(p => p.UserId == id).Count() > 0)
            {
                var fb = _db.FeedBacks.Where(p => p.UserId == id).FirstOrDefault();
                //Remove Feedbackdetails
                while (_db.FeedBackDetails.Where(p => p.FeedbackId == fb.Id).Count() > 0)
                {
                    var fbdt = _db.FeedBackDetails.Where(p => p.FeedbackId == fb.Id).FirstOrDefault();
                    _db.Entry(fbdt).State = EntityState.Deleted;
                    _db.SaveChanges();
                };

                //Remove Link Feedback
                var l = _db.Links.Where(p => p.FeedbackId == fb.Id).FirstOrDefault();
                if (l != null) _db.Entry(l).State = EntityState.Deleted;
                _db.Entry(fb).State = EntityState.Deleted;
                _db.SaveChanges();

            };

            _db.Entry(user).State = EntityState.Deleted;
            _db.SaveChanges();
            return RedirectToAction("UserManagement");
        }
    }


}
