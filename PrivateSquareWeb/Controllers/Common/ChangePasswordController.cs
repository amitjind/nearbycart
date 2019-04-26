using Newtonsoft.Json;
using PrivateSquareWeb.CommonCls;
using PrivateSquareWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace PrivateSquareWeb.Controllers
{
    public class ChangePasswordController : Controller
    {
        JwtTokenManager _JwtTokenManager = new JwtTokenManager();
        static bool IsChangePassword = false;
        // GET: ChangePassword
        public ActionResult Index()
        {
            if (IsChangePassword)
            {
                Thread.Sleep(5000);
                return RedirectToAction("Index", "Home");
            }
            return View();

        }
        [HttpPost]
        public ActionResult ChangePassword(LoginModel ObjModel)
        {
            if (ModelState.IsValid)
            {
                LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);
                if (MdUser.Id != 0)
                {
                    ObjModel.Id = Convert.ToInt64(MdUser.Id);
                }
                string PasswordEncripy = CommonFile.EncodePasswordMd5(ObjModel.NewPassword);
                string PasswordEncripy2 = CommonFile.EncodePasswordMd5(ObjModel.Password);
                ObjModel.ConfirmNewPassword = PasswordEncripy;
                ObjModel.NewPassword = PasswordEncripy;
                ObjModel.Password = PasswordEncripy2;
                var _request = JsonConvert.SerializeObject(ObjModel);
                ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiChangePassword, _request);
                ResponseModel ObjResponse1 = JsonConvert.DeserializeObject<ResponseModel>(ObjResponse.Response);


                if (String.IsNullOrWhiteSpace(ObjResponse.Response))
                {
                    return View("Index", ObjModel);
                }
                if (ObjResponse1.Response.Equals("Wrong Password"))
                {
                    ViewBag.ResponseMessage = "Your Current Password is Wrong";
                    return View("Index", ObjModel);
                }
                else
                {
                    ViewBag.ResponseMessage = "Your Password has been changed Please Login ";
                    Services.RemoveCookie(this.ControllerContext.HttpContext, "usr");
                    HeaderPartialModel objModel = new HeaderPartialModel();
                    objModel.UserName = "";
                    objModel.UserId = 0;
                    objModel.ProfileImg = "";
                    IsChangePassword = true;
                    return View("Index", ObjModel);
                    // return RedirectToAction("Index","Login");
                }

            }

            return View("Index", ObjModel);
        }
        public ActionResult Next()
        {
            HeaderPartialModel objModel = new HeaderPartialModel();
            objModel.UserName = "";
            objModel.UserId = 0;
            objModel.ProfileImg = "";
            return PartialView("~/Views/Shared/_Header.cshtml", objModel);
        }

        public ActionResult WebIndex()
        {
            return View("WebHomeChangePassword");
        }
        [HttpPost]
        public ActionResult WebHomeChangePassword(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);
                if (MdUser.Id != 0)
                {
                    loginModel.Id = Convert.ToInt64(MdUser.Id);
                }
                string PasswordEncripy = CommonFile.EncodePasswordMd5(loginModel.NewPassword);
                string PasswordEncripy2 = CommonFile.EncodePasswordMd5(loginModel.Password);
                loginModel.ConfirmNewPassword = PasswordEncripy;
                loginModel.NewPassword = PasswordEncripy;
                loginModel.Password = PasswordEncripy2;
                var _request = JsonConvert.SerializeObject(loginModel);
                ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiChangePassword, _request);
                ResponseModel ObjResponse1 = JsonConvert.DeserializeObject<ResponseModel>(ObjResponse.Response);


                if (String.IsNullOrWhiteSpace(ObjResponse.Response))
                {
                    return View(loginModel);
                }
                if (ObjResponse1.Response.Equals("Wrong Password"))
                {
                    ViewBag.WebResponseMessage = "Your Current Password is Wrong";
                    return View(loginModel);
                }
                else
                {
                    ViewBag.WebResponseMessage = "Your Password has been changed Please Login ";
                    Services.RemoveCookie(this.ControllerContext.HttpContext, "webusr");
                    HeaderPartialModel objModel = new HeaderPartialModel();
                    objModel.UserName = "";
                    objModel.UserId = 0;
                    objModel.ProfileImg = "";
                    IsChangePassword = true;
                    return RedirectToAction("Index","WebLogin");
                }
               
            }
            return View("WebHomeChangePassword");
        }
    }
}