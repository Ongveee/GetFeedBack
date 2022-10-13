using GetFeedBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System;
using System.Text.Encodings.Web;
 
namespace GetFeedBack.Controllers
{
    [Authorize(Role.User)]
    public class FeedBackController : BaseController
    {
        private readonly FeedbackContext _db;
        public FeedBackController(FeedbackContext db)
        {
            _db = db;
        }

        public IActionResult Index(int id)
        {
            List<FeedBacks> feedBacks = _db.FeedBacks.Where(p => p.UserId == id).ToList();
            TempData["UserId"] = id;
            return View(feedBacks);

        }
        [HttpGet]
        public IActionResult Create(int id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int id, string name, string description)
        {
            FeedBacks feedBack = new FeedBacks();
            if (ModelState.IsValid)
            {
                feedBack.UserId = id;
                feedBack.Name = name;
                feedBack.Description = description;
                feedBack.CreateDate = DateTime.Now;
                _db.FeedBacks.Add(feedBack);
                _db.SaveChanges();
                return RedirectToAction("Index", new { id = feedBack.UserId });
            }
            return NotFound(feedBack);
        }

        public IActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            FeedBacks feedBacks = _db.FeedBacks.Where(fb => fb.Id == Id).FirstOrDefault();
            if (feedBacks == null)
            {
                return NotFound();
            }
            return View(feedBacks);
        }
        [HttpGet]
        public IActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            FeedBacks feedBacks = _db.FeedBacks.Where(fb => fb.Id == Id).FirstOrDefault();
            if (feedBacks == null)
            {
                return NotFound();
            }
            return View(feedBacks);
        }
        [HttpPost]
        public IActionResult Edit(int? Id, FeedBacks feedBacks)
        {
            if (ModelState.IsValid)
            {
                var fb = _db.FeedBacks.FirstOrDefault(p => p.Id == feedBacks.Id);
                fb.Name = feedBacks.Name;
                fb.Description = feedBacks.Description;
                _db.SaveChanges();
                return RedirectToAction("Index", new { id = fb.UserId });
            }
            return NotFound();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var fb = _db.FeedBacks.FirstOrDefault(fb => fb.Id == id);
            var UserId = fb.UserId;
            //Remove all FeedbackDetails 
            while (_db.FeedBackDetails.Where(p => p.FeedbackId == id).Count() > 0) 
            {   
                var fbdt = _db.FeedBackDetails.FirstOrDefault(p => p.FeedbackId == fb.Id);
                _db.Entry(fbdt).State = EntityState.Deleted;
                _db.SaveChanges();
            };
            

            //Remove Link Feedback
            var l = _db.Links.Where(p => p.FeedbackId == id).FirstOrDefault();
            if (l != null) _db.Entry(l).State = EntityState.Deleted;
            _db.Entry(fb).State = EntityState.Deleted;
            _db.SaveChanges();
            return RedirectToAction("Index", new { id = UserId });
        }
        [HttpGet]
        public IActionResult CreateLink(int id)
        {
            var fb = _db.FeedBacks.Where(p => p.Id == id).FirstOrDefault();
            var UserId = fb.UserId;
            TempData["UserID"] = fb.UserId;
            if (_db.Links.Where(p => p.FeedbackId == id).FirstOrDefault() == null)
            {
                Links l = new Links();
                l.FeedbackId = id;
            }
            else
            {
                var link = _db.Links.FirstOrDefault(p => p.FeedbackId == id);
                string ms = $"Đã có link: {link.Link}";
                ViewData["message"] = ms;
            }


            return View();
        }

        public IActionResult CreateLink(int id, DateTime deadline)
        {

            
            Links l = new Links();
            l.FeedbackId = id;
            l.Deadline = deadline;
            l.Link = $"https://localhost:44381/FeedBackForm/FeedBackForm/{id}";
            _db.Links.Add(l);
            _db.SaveChanges();
            TempData["SuccessMessage"] = $"Link của bạn nè: {l.Link}";
            var fb = _db.FeedBacks.Where(p => p.Id == l.FeedbackId).FirstOrDefault();
            TempData["UserId"] = fb.UserId;
            return View();
        }

        

    }

}
