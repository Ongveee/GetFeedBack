using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GetFeedBack.Models
{
    public partial class FeedBacks
    {
        public FeedBacks()
        {
            FeedBackDetails = new HashSet<FeedBackDetails>();
            Links = new HashSet<Links>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? UserId { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<FeedBackDetails> FeedBackDetails { get; set; }
        public virtual ICollection<Links> Links { get; set; }
    }
}
