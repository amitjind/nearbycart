using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateSquareWeb.Models
{
    public class ProductImages
    {
        public String Name { get; set; }
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public HttpPostedFileBase ImageUpload { get; set; }
        public string ImageUrl { get; set; }

    }
}