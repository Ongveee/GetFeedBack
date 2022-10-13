using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GetFeedBack.Models
{
    public partial class Users
    {
        public Users()
        {
            FeedBacks = new HashSet<FeedBacks>();
        }
        
        [Required(ErrorMessage = "Id is required")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required( ErrorMessage = "Please enter the name")]
        [StringLength(maximumLength: 25, MinimumLength = 10, ErrorMessage = "Length must be between 10 to 25")]
        [Display(Name = "UserName")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please Enter Password...")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public DateTime CreateDate { get; set; }
        public virtual ICollection<FeedBacks> FeedBacks { get; set; }
    }
}
