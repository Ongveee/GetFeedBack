using GetFeedBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace GetFeedBack.DataAccess
{
    public class FeedBackDAL : IFeedBackDAL
    {
        private readonly FeedbackContext _db;
        public FeedBackDAL(FeedbackContext db)
        {
            _db = db;
        }
        public IEnumerable<FeedBacks> GetFeedBackList()
        {
            var feedbacks = (from fb in _db.FeedBacks
                           select new FeedBacks
                           {
                               Id=fb.Id,
                               Name=fb.Name,
                           }).ToList();
            return feedbacks;
        }
        public bool AddFeedBackDetails(FeedBacks feedBacks)
        {
            using (var transcation = _db.Database.BeginTransaction())
            {
                try
                {
                    var feedbacks = new FeedBacks
                    {
                        Id = feedBacks.Id,
                        Name = feedBacks.Name
                    };
                    _db.FeedBacks.Add(feedbacks);
                    _db.SaveChanges();
                    transcation.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transcation.Rollback();
                    return false;
                }
            }
        }
        public FeedBacks GetFeedBackById(int Id)
        {
            var feedback =_db.FeedBacks.FirstOrDefault(fb => fb.Id == Id);
            if(feedback != null)
            {
                var model = new FeedBacks
                {
                    Id = feedback.Id,
                    Name = feedback.Name,
                };
                return model;
            }
            return new FeedBacks();
        }
        public bool EditFeedBack(FeedBacks feedBacks)
        {
            using (var transcation = _db.Database.BeginTransaction())
            {
                try
                {
                    var data = _db.FeedBacks.FirstOrDefault(fb => fb.Id == feedBacks.Id);
                    if (data != null)
                    {
                        data.Name = feedBacks.Name;
                        _db.FeedBacks.Update(data);
                        _db.SaveChanges();
                        transcation.Commit();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    transcation.Rollback();
                    return false;
                }
            }
        }
        public bool DeleteFeedBack(int Id)
        {
            using(var transcation = _db.Database.BeginTransaction())
            {
                try
                {
                    var data = _db.FeedBacks.FirstOrDefault(fb => fb.Id == Id);
                    if (data != null)
                    {
                        _db.FeedBacks.Remove(data);
                        _db.SaveChanges();
                        transcation.Commit();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    transcation.Rollback();
                    return false;
                }
            }
        }     
    }
}
