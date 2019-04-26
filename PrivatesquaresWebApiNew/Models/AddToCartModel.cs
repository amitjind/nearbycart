using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivatesquaresWebApiNew.Models
{
    public class AddToCartModel
    {
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public string ImageName { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public string XmlCartDetails { get; set; }
        //public string XmlWishlistDetails { get; set; }
        public string  Operation { get; set; }
    }
}