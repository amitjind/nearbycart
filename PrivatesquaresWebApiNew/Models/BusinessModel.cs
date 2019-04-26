using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivatesquaresWebApiNew.Models
{
    public class BusinessModel
    {
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string Location { get; set; }
        public string BusinessLogo { get; set; }
        public long ProfessionalCatId { get; set; }
        public string ProfessionalKeyword { get; set; }
        public long CityId { get; set; }
        public string PinCode { get; set; }
        public long? UserId { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public long CountryId { get; set; }
        public string Phone { get; set; }
        public string Operation { get; set; }
        public string Website { get; set; }
    }
}