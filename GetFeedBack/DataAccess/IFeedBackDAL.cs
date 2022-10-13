using GetFeedBack.Models;
using System.Collections.Generic;

namespace GetFeedBack.DataAccess
{
    public interface IFeedBackDAL
    {
        IEnumerable<FeedBacks> GetFeedBackList();
        bool AddFeedBackDetails(FeedBacks feedBacks);
        FeedBacks GetFeedBackById(int Id);
        bool EditFeedBack(FeedBacks feedBacks);
        bool DeleteFeedBack(int Id);
    }
}
