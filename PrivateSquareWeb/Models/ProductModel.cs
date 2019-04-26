using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrivateSquareWeb.Models
{
    public class ProductModel
    {
        public long Id { get; set; }
        public string Sortby { get; set; }                                  //field used for sorting products
        public long PageIndex { get; set; }                                //field used for pagination of products while showing results in sorting
        public long ProductId { get; set; }                               // extra productId field for binding the ViewBag.Wishlist
        [Required(ErrorMessage = "Product Name cannot be empty")]
        public string ProductName { get; set; }
        public long? ProductCatId { get; set; }
        public long? ParentCatId { get; set; }
        public string ProductImage { get; set; }
        [Required(ErrorMessage ="Selling Price cannot be empty")]
        public decimal SellingPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public long BusinessId { get; set; }
        public long? UserId { get; set; }
        public string Description { get; set; }
        public string XmlProductImage { get; set; }
        public string ProductImages { get; set; }
        public string VendorName { get; set; }
        public string Operation { get; set; }
        //public int SearchResultCount { get; set; }
        //Below are the properties for pagination
        public long SearchResultCount { get; set; } = 0;
        public long LowerLimit { get; set; } = 0;
        public long NumberOfPages { get; set; } = 0;
        public string CategoryName { get; set; }
    }
}