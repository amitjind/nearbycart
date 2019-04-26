using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateSquareWeb.Models
{
    public class LoginModel
    {
        public long Id { get; set; } = 0;
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Please Enter your password")]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string ProfileImg { get; set; } = "";
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Please Enter your new password")]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Please confirm your Password")]
        [Display(Name = "Re-type Password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmNewPassword { get; set; }
        public string RegisterType { get; set; }
        public string Operation { get; set; }

    }
}