using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateSquareWeb.Models
{
    public class DropDownModel
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public String EncodedId { get; set; }
        public String CatIdEncoded{ get; set; }
        public long? CountryId { get; set; }
        public long? StateId { get; set; }
        public long? ParentCatId { get; set; }
    }
}