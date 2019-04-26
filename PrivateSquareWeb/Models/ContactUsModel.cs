using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateSquareWeb.Models
{
    public class ContactUsModel
    {
        [Required(ErrorMessage = "Name can't  be empty")]
        public string FullName { get; set; }
        //[Required(ErrorMessage = "Mobile No. can't  be empty")]
        [MinLength(10, ErrorMessage = "Enter 10 Digit")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile must be numeric")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Email can't  be empty")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Message can't  be empty")]
        public string Message { get; set; }

        public long UserId { get; set; }   //UserId for Reviews
        public long ProductId { get; set; }   //ProductId for Reviews
        public decimal TotalRating { get; set; }    //field used for Reviews
        public decimal GivenRating { get; set; }   //field used for Reviews
        public string Review { get; set; }     //field used for Reviews
        public string Name { get; set; }     //field used for Reviews
        public string RecordDate { get; set; }  //field used for displaying date of review in Reviews section in Product Details Page
        public string Operation { get; set; }   //field used for Reviews
    }
}