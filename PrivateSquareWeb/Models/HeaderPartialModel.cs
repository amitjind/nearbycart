using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateSquareWeb.Models
{
    public class HeaderPartialModel
    {
        public long UserId { get; set; } = 0;
        public string UserName { get; set; } = "Not Login";
        public string ProfileImg { get; set; } = "";
        public int CartItemCount { get; set; } = 0;
        public string SearchBarText { get; set; }
        public long ParentCatId { get; set; }
    }
}