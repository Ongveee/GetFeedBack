using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GetFeedBack.Models
{
    public partial class Links
    {

        public int Id { get; set; }
        public DateTime Deadline { get; set; }
        public int? FeedbackId { get; set; }
        public string Link { get; set; }

        public virtual FeedBacks Feedback { get; set; }
    }
}
