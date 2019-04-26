using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivatesquaresWebApiNew.Models
{
    public class UserRegisterModel
    {
        public long Id { get; set; }
        public string Mobile { get; set; }
        public string Otp { get; set; }
        public string Operation { get; set; }
    }
}