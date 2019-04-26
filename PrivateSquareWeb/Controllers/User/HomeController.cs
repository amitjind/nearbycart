using ASPSnippets.FaceBookAPI;
using Newtonsoft.Json;
using PrivateSquareWeb.CommonCls;
using PrivateSquareWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PrivateSquareWeb.Controllers
{
    public class HomeController : Controller
    {
        JwtTokenManager _JwtTokenManager = new JwtTokenManager();
        public ActionResult Index()
        {
            List<UsersProfileModel> usersList = GetAllUsers();

            ViewBag.AllUsers = usersList;
            #region For Facebook Login
            // For Facebook Login
            /*
            FaceBookUser faceBookUser = new FaceBookUser();
            if (Request.QueryString["error"] == "access_denied")
            {
                ViewBag.Message = "User has denied access.";
            }
            else
            {
                string code = Request.QueryString["code"];
                if (!string.IsNullOrEmpty(code))
                {
                    string data = FaceBookConnect.Fetch(code, "me?fields=id,name,email");
                    faceBookUser = new JavaScriptSerializer().Deserialize<FaceBookUser>(data);
                    faceBookUser.PictureUrl = string.Format("https://graph.facebook.com/{0}/picture", faceBookUser.Id);
                }
            }
            */
            #endregion
            return View();
        }
        public List<UsersProfileModel> GetAllUsers()
        {
            var GetAllUserList = new List<UsersProfileModel>();
            UsersProfileModel objUserModel = new UsersProfileModel();
            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);

            if (MdUser.Id != 0)
            {
                objUserModel.UserId = MdUser.Id; //Convert.ToInt64(Services.GetCookie(this.ControllerContext.HttpContext, "usrId").Value);
            }
            var _request = JsonConvert.SerializeObject(objUserModel);
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetUsersProfile, _request);
            GetAllUserList = JsonConvert.DeserializeObject<List<UsersProfileModel>>(ObjResponse.Response);
            return GetAllUserList;

        }
        public List<BusinessModel> GetUsersBusiness()
        {
            var GetUserBusinessList = new List<BusinessModel>();
            BusinessModel objmodel = new BusinessModel();

            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            if (MdUser.Id != 0)
                objmodel.UserId = Convert.ToInt64(MdUser.Id);
            var _request = JsonConvert.SerializeObject(objmodel);
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetUserBusiness, _request);
            GetUserBusinessList = JsonConvert.DeserializeObject<List<BusinessModel>>(ObjResponse.Response);
            return GetUserBusinessList;

        }
       
        public List<UsersProfileModel> GetUsersNetwork()
        {
            var GetUserNetworkList = new List<UsersProfileModel>();
            NetworkModel objmodel = new NetworkModel();
            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);

            if (MdUser.Id != 0)
                objmodel.LogInUserId = Convert.ToInt64(MdUser.Id);
            var _request = JsonConvert.SerializeObject(objmodel);
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetUserNetwork, _request);
            GetUserNetworkList = JsonConvert.DeserializeObject<List<UsersProfileModel>>(ObjResponse.Response);
            return GetUserNetworkList;

        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult MyBusinessList()
        {
            //if (!CommonFile.IsUserAuthenticate(this.ControllerContext.HttpContext))
            //{
            //    return RedirectToAction("Index", "Login");
            //}

            // ViewBag.UsersBusiness = GetUsersBusiness();
            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            long UserId = 0;
            if (MdUser.Id != 0)
                UserId = MdUser.Id;
             ViewBag.UsersBusiness =CommonFile.GetUsersBusiness(UserId);

            return View();
        }
       
        public ActionResult NetworkList()
        {
            //if (!CommonFile.IsUserAuthenticate(this.ControllerContext.HttpContext))
            //{
            //    return RedirectToAction("Index", "Login");
            //}
            ViewBag.UsersNetwork = GetUsersNetwork();
            return View();
        }
        [HttpPost]
        public JsonResult AddNetwork(NetworkModel objNetworkModel)
        {
            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);

            if (MdUser.Id != 0)
            {
                objNetworkModel.LogInUserId = Convert.ToInt64(MdUser.Id);

                objNetworkModel.Operation = "insert";
                var _request = JsonConvert.SerializeObject(objNetworkModel);
                ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiSaveNetwork, _request);
                if (String.IsNullOrWhiteSpace(ObjResponse.Response))
                {
                }
                String Response = "[{\"Response\":\"" + ObjResponse.Response + "\"}]";
                return Json(Response);
            }
            else
            {
                String Response = "[{\"Response\":\"" + " NotLogin" + "\"}]";
                return Json(Response);
            }
        }
    }
}