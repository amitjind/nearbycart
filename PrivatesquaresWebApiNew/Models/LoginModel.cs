using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivatesquaresWebApiNew.Models
{
    public class LoginModel
    {
        public long Id { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string ProfileImg { get; set; }
        public string Mobile { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
        public string Operation { get; set; }
        public string RegisterType { get; set; }
    }
}