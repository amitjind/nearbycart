using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateSquareWeb.Models
{
    public class UserProfileModel
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        [Required(ErrorMessage = "First Name is Required.")]

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfileImage { get; set; }



        public string Description { get; set; }

        [Required(ErrorMessage = "Email is Required.")]

        [EmailAddress(ErrorMessage = "Invalid Email Address")]

        public string EmailId { get; set; }

        public long ProfessionalCatId { get; set; }

        public string Title { get; set; }

        public string ProfessionalKeyword { get; set; }

        [Required(ErrorMessage = "Please Select City")]

        public long CityId { get; set; }

        public string Password { get; set; }

        public long GenderId { get; set; }

        //[DataType(DataType.Date)]

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        public DateTime DOB { get; set; }



        public string Location { get; set; }

        [Required(ErrorMessage = "Phone is Required.")]

        public string Phone { get; set; }

        public string Pincode { get; set; }

        public long CountryId { get; set; }

        public long StateId { get; set; }

        public long InterestCatId { get; set; }

        public string OfficeAddress { get; set; }

        public string OtherAddress { get; set; }



        public int[] UserInterestIds { get; set; }

        public string StrUserInterestIds { get; set; }

        public string XmlData { get; set; }

        public string XmlDataAddress { get; set; }

        public string StrUserAddress { get; set; }
    }
}