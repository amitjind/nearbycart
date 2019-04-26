using Newtonsoft.Json;
using PrivateSquareWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateSquareWeb.Controllers.Website
{
    public class ViewCartController : Controller
    {
        JwtTokenManager _JwtTokenManager = new JwtTokenManager();
        // GET: ViewCart
        public ActionResult ViewCart()
        {
            List<AddToCartModel> ListAddToCart = Services.GetMyCart(this.ControllerContext.HttpContext, _JwtTokenManager);
            ViewBag.AddToCart = ListAddToCart;
            ViewBag.TotalAmount = GetTotalAmount(ListAddToCart);
            return View();
        }
        public JsonResult RemoveToCart(int index)
        {
            AddToCart objAddToCart = new AddToCart();
            return objAddToCart.RemoveCart(index, this.ControllerContext.HttpContext);

        }

        public decimal GetTotalAmount(List<AddToCartModel> ListCart)
        {
            AddToCart objAddToCart = new AddToCart();
            return objAddToCart.GetTotalAmount(ListCart);
        }
    }
}