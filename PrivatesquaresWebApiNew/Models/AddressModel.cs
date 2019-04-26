using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivatesquaresWebApiNew.Models
{
    public class AddressModel
    {
        public long? Id { get; set; }
        public string Address { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Pincode { get; set; }
        public string Locality { get; set; }
        public long CityId { get; set; }
        public string CityName { get; set; }
        public long StateId { get; set; }
        public string StateName { get; set; } = "";
        public string Landmark { get; set; }
        public string AlternatePhone { get; set; }
        public string Type { get; set; }
        public string Operation { get; set; }

    }
}