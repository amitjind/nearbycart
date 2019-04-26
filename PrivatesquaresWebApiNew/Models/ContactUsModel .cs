using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivatesquaresWebApiNew.Models
{
    public class ContactUsModel
    {
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
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