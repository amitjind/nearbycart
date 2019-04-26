using PrivateSquareWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateSquareWeb.Controllers
{
    public class SidebarProfileController : Controller
    {
        JwtTokenManager _JwtTokenManager = new JwtTokenManager();

        // GET: SidebarProfile

        public ActionResult Index()

        {

            return View();

        }

        public PartialViewResult HeaderValue()
{
            AddressModel objModel = new AddressModel();
            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            if (MdUser.Id != 0)
            {
                objModel.Name = MdUser.Name;
                objModel.UserId = Convert.ToInt64(MdUser.Id);

                objModel.ProfileImg = MdUser.ProfileImg;

            }

            else

            {



            }

            return PartialView("~/Views/Shared/_Sidebar.cshtml", objModel);

        }
    }
}