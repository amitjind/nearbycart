using Newtonsoft.Json;
using PrivateSquareWeb.CommonCls;
using PrivateSquareWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateSquareWeb.Controllers.Website
{
    public class WebSidebarController : Controller

    {
        JwtTokenManager _JwtTokenManager = new JwtTokenManager();

        // GET: WebSidebar
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult WebSiteSidebar()
        {
            ProductModel objModel = new ProductModel();
            var ProductCatList = CommonFile.GetProductCategory(null);
            ViewBag.ProductCatList = ProductCatList;
            List<AddToCartModel> ListAddToCart = Services.GetMyCart(this.ControllerContext.HttpContext, _JwtTokenManager);
            ViewBag.AddToCart = ListAddToCart;
            objModel.CartItemCount = ListAddToCart.Count();
            ViewBag.TotalAmount = GetTotalAmount(ListAddToCart);
            return PartialView("~/Views/Shared/_WebSiteSidebar.cshtml", objModel);
        }

        public decimal GetTotalAmount(List<AddToCartModel> ListCart)
        {
            AddToCart objAddToCart = new AddToCart();
            return objAddToCart.GetTotalAmount(ListCart);
        }
    }
}