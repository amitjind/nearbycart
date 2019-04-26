using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateSquareWeb.Models
{
    public class RequestModel
    {
        public string Data { get; set; }
        public int PageNumber { get; set; }
        public string orderBy { get; set; }
        public bool ShowAll { get; internal set; }
    }
}