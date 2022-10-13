using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GetFeedBack.Models
{
    public partial class FeedBackDetails
    {
        public int Id { get; set; }
        public string Advantage { get; set; }
        public string Disavantage { get; set; }
        public string Opinion { get; set; }
        public int? FeedbackId { get; set; }
        public string SenderName { get; set; }

        public virtual FeedBacks Feedback { get; set; }
    }
}
