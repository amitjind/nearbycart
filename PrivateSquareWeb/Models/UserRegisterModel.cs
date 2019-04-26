using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateSquareWeb.Models
{
    public class UserRegisterModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Mobile number is required.")]
        // [MaxLength(12,ErrorMessage ="Please Enter Less Then 12 Character")]
        [MinLength(10,ErrorMessage ="Enter 10 Digit")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile must be numeric")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "OTP is required.")]
       // [MaxLength(6, ErrorMessage = "Please Enter Less Then 6 Character")]
        [MinLength(4, ErrorMessage = "Enter 4 Digit OTP Number.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "OTP must be numeric")]
        public string Otp { get; set; }
        public string Operation { get; set; }
    }
}