using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateSquareWeb.Models
{
    public class NetworkModel
    {
        public long LogInUserId { get; set; }
        public long UserId { get; set; }
        public long UserProfileId { get; set; }

        public string Operation { get; set; }
    }
}