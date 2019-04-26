using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivatesquaresWebApiNew.Models
{
    public class AllInterestModel
    {
        public long InterestId { get; set; }
        public string InterestName { get; set; }
        public long InterestCatId { get; set; }
        public string InterestCatName { get; set; }
       
    }
}