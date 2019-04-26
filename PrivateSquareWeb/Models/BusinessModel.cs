using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateSquareWeb.Models
{
    public class BusinessModel
    {
        public long? Id { get; set; }= 0;

        [Required(ErrorMessage = "Name is Required")]
        public string BusinessName { get; set; } = "";

        [Required(ErrorMessage ="Business Location cannot be empty !")]
        public string Location { get; set; } = "";
        public string BusinessLogo { get; set; } = "";
        public long ProfessionalCatId { get; set; } = 0;
        public string ProfessionalKeyword { get; set; } = "";

        [Required(ErrorMessage ="Please select a city")]
        public long CityId { get; set; } = 0;
        public string PinCode { get; set; } = "";
        public long UserId { get; set; } = 0;

        [Required(ErrorMessage = "Email cannot be empty")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = "";

        public string Description { get; set; } = "";

        [Required(ErrorMessage ="Please select a country")]
        public long CountryId { get; set; } = 0;

        [Required(ErrorMessage ="Please Provide a contact number for your Business")]
        [MinLength(10,ErrorMessage ="Minimum 10 digits (Enter area code for landline)")]
        public string Phone { get; set; } = "";

        //[Required(ErrorMessage ="Please enter a state")]
        //public long StateId { get; set; } = 0;
        public string Operation { get; set; } = "";
        public string Website { get; set; }
    }
}