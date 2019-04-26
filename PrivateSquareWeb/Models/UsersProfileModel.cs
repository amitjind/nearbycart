using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateSquareWeb.Models
{
    public class UsersProfileModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public String Name { get; set; }
        public string ProfileImage { get; set; }
        public string Description { get; set; }
        public string EmailId { get; set; }
        public long ProfessionalCatId { get; set; }
        public string Profession { get; set; }
        public string Title { get; set; }
        public string ProfessionalKeyword { get; set; }
        public long CityId { get; set; }
        public string InterestId { get; set; }
        public string InterestName { get; set; }
        public string Location { get; set; }
    }
}