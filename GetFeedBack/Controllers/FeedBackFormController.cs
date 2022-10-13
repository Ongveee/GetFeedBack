using GetFeedBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace GetFeedBack.Controllers
{
    public class FeedBackFormController : Controller
    {
        private readonly FeedbackContext _db;

        public FeedBackFormController(FeedbackContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult FeedbackForm(int id)
        {
            if (_db.FeedBacks.Where(p => p.Id == id).FirstOrDefault() == null) return NotFound();
            else
            {
                var l = _db.Links.FirstOrDefault(p => p.FeedbackId == id);
                if (DateTime.Compare(DateTime.Now, l.Deadline) > 0) return NotFound();
            }
            return View();
        }

        [HttpPost]
        public IActionResult FeedbackForm(int id, string Sendername, string Advantage, string Disavantage, string Opinion)
        {

            if (ModelState.IsValid)
            {
                var fb = _db.FeedBacks.FirstOrDefault(p => p.Id == id);
                TempData["SuccessMessage"] = $"Cảm ơn bạn đã phản hồi về {fb.Name}";
                FeedBackDetails fbdt = new FeedBackDetails();
                fbdt.FeedbackId = id;
                fbdt.SenderName = Sendername;
                fbdt.Advantage = Advantage;
                fbdt.Disavantage = Disavantage;
                fbdt.Opinion = Opinion;
                _db.FeedBackDetails.Add(fbdt);
                _db.SaveChanges();
                return View(fbdt);
            }
            return NotFound();

        }
    }
}
