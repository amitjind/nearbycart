using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateSquareWeb.Models
{
    public class UserInterestModel
    {
        public long UserId { get; set; }
        public long InterestId { get; set; }
        public long InterestCatId { get; set; }
        public string XmlData { get; set; }
        public string InterestName { get; set; }

        public string InterestCatName { get; set; }
        public string Operation { get; set; }
    }
}