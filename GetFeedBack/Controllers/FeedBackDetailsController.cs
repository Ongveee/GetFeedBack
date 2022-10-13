using GetFeedBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace GetFeedBack.Controllers
{
    [Authorize(Role.User)]
    public class FeedBackDetailsController : BaseController
    {
        private readonly FeedbackContext _db;
        public FeedBackDetailsController(FeedbackContext db)
        {
            _db = db;
        }
        public IActionResult Index(int? Id)
        {
            List<FeedBackDetails> feedBackDetails = _db.FeedBackDetails.Where(fbdt => fbdt.FeedbackId == Id).ToList();
            var fb = _db.FeedBacks.FirstOrDefault(p => p.Id == Id);
            var userId = fb.UserId;
            TempData["UserId"] = userId;
            return View(feedBackDetails);
        }
        public IActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            FeedBackDetails feedBackDetails = _db.FeedBackDetails.Where(fb => fb.Id == Id).FirstOrDefault();
            if (feedBackDetails == null)
            {
                return NotFound();
            }
            return View(feedBackDetails);
        }
        [HttpGet]
        public IActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            FeedBackDetails feedBackDetails = _db.FeedBackDetails.Where(fb => fb.Id == Id).FirstOrDefault();
            if (feedBackDetails == null)
            {
                return NotFound();
            }
            return View(feedBackDetails);
        }
        [HttpPost]
        public IActionResult Delete(FeedBackDetails feedBackDetails)
        {
            var delete = _db.FeedBackDetails.FirstOrDefault(x => x.Id == feedBackDetails.Id);
            _db.Entry(delete).State = EntityState.Deleted;
            _db.SaveChanges();
            return RedirectToAction("Index", new { Id = delete.FeedbackId });
        }
    }
}
