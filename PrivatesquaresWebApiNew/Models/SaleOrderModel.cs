using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivatesquaresWebApiNew.Models
{
    public class SaleOrderModel
    {

        public long Id { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ProductImages { get; set; }
        public string Quantity { get; set; }
        public string VendorName { get; set; }
        public long UserId { get; set; }
        public long? SaleOrderId { get; set; }

        public long? ProductId { get; set; }
        public string PaymentMode { get; set; }

        public string XmlSaleOrderDetail { get; set; }

        public decimal TotalDiscount { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Operation { get; set; }
    }
}