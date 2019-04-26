using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateSquareWeb.Models
{
    public class AddressModel
    {

        public long? Id { get; set; }
        public string Address { get; set; }
        public long UserId { get; set; }
        [Required(ErrorMessage = "Name can't  be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Mobile No. can't  be empty")]
        [MinLength(10, ErrorMessage = "Enter 10 Digit Mobile Number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile must be numeric")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Pincode cannot be empty")]
        [MinLength(6, ErrorMessage = "Please enter 6 digits Pin")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be numeric")]
        public string Pincode { get; set; }
        [Required(ErrorMessage = "Please enter locality")]
        public string Locality { get; set; }
        public long CityId { get; set; }
        public string CityName { get; set; }
        public long StateId { get; set; }
        public string StateName { get; set; } = "";
        public string Landmark { get; set; }
        public string AlternatePhone { get; set; }
        public string Type { get; set; }
        public string ProfileImg { get; set; }
        public string Operation { get; set; }
    }
}