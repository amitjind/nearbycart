using PrivateSquareWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateSquareWeb.Controllers
{
    public class HeaderPartialController : Controller
    {
        JwtTokenManager _JwtTokenManager = new JwtTokenManager();
        // GET: HeaderPartial
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult HeaderValue()
        {
            HeaderPartialModel objModel = new HeaderPartialModel();
            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            List<AddToCartModel> ListAddToCart = Services.GetMyCart(this.ControllerContext.HttpContext, _JwtTokenManager);
            if (MdUser.Id != 0)
            {
                objModel.UserName =MdUser.Name;
                objModel.UserId = Convert.ToInt64(MdUser.Id);
                objModel.ProfileImg = MdUser.ProfileImg;
                objModel.CartItemCount = ListAddToCart.Count();
            }
            else
            {

            }
            return PartialView("~/Views/Shared/_Header.cshtml", objModel);
        }
    }
}