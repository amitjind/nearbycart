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
    public class WebLoginController : Controller
    {
        JwtTokenManager _JwtTokenManager = new JwtTokenManager();
        // GET: WebLogin
        public ActionResult Index()
        {
            Services.RemoveCookie(this.ControllerContext.HttpContext, "webusr");
            return View();
        }
        public ActionResult Logout()
        {
            Services.RemoveCookie(this.ControllerContext.HttpContext, "webusr");
            return RedirectToAction("Index", "WebLogin");
            //return View();
        }
        [HttpPost]
        public ActionResult LoginUser(LoginModel ObjModel)
        {
            //var abc = ObjModel.EmailId;
            ObjModel.EmailId = (ObjModel.EmailId).Replace(" ", "");
            if (string.IsNullOrWhiteSpace(ObjModel.EmailId))
            {
                ModelState.AddModelError("EmailId", "Email Or Mobile Required");
                return View("Index", ObjModel);
            }
            if (string.IsNullOrWhiteSpace(ObjModel.Password))
            {
                ModelState.AddModelError("Password", "Password Required");
                return View("Index", ObjModel);
            }

            string res;
            long a;
            string myStr = ObjModel.EmailId;
            res = Int64.TryParse(myStr, out a).ToString();
            if (res == "True")
            {
                ObjModel.Mobile = ObjModel.EmailId;
                if (ObjModel.Mobile.Length != 10)
                {
                    ModelState.AddModelError("EmailId", "Mobile Number Incorrect");
                    return View("Index", ObjModel);
                }
                ObjModel.EmailId = null;
            }
            else
            {
                bool IsValidEmail = CommonFile.ValidateEmailIsValid(ObjModel.EmailId);
                if (!IsValidEmail)
                {
                    ModelState.AddModelError("EmailId", "Email / Mobile Incorrect");
                    return View("Index", ObjModel);
                }

                ObjModel.Mobile = null;
            }

            //Password Encode
            string PasswordEncripy = CommonFile.EncodePasswordMd5(ObjModel.Password);
            ObjModel.Password = PasswordEncripy;
            /////////
            var _request = _JwtTokenManager.GenerateToken(JsonConvert.SerializeObject(ObjModel));
            ResponseModel ObjResponse = CommonFile.GetApiResponseJWT(Constant.ApiLoginUser, _request);
            ResponseModel ObjResponse1 = JsonConvert.DeserializeObject<ResponseModel>(ObjResponse.Response);
            String VarResponse = ObjResponse1.Response;
            if (VarResponse.Equals("Email/Password is Incorrect"))
            {
                ViewBag.Response = "Email/Password is Incorrect";
                return View("Index", ObjModel);
            }
            else if (VarResponse.Equals("Phone/Password is Incorrect"))
            {
                ViewBag.Response = "Phone/Password is Incorrect";
                return View("Index", ObjModel);
            }
            else
            {
                string[] ArrResponse = VarResponse.Split(',');

                var jsonString = "{\"Id\":\"" + ArrResponse[0] + "\",\"Name\":\"" + ArrResponse[1] + "\",\"ProfileImg\":\"" + ArrResponse[2] + "\",\"EmailId\":\"" + ArrResponse[3] + "\",\"Mobile\":\"" + ArrResponse[4] + "\"}";
                Services.SetCookie(this.ControllerContext.HttpContext, "webusr", _JwtTokenManager.GenerateToken(jsonString.ToString()));


                //Services.SetCookie(this.ControllerContext.HttpContext, "usrId", ArrResponse[0]);
                //Services.SetCookie(this.ControllerContext.HttpContext, "usrName", ArrResponse[1]);
                //Services.SetCookie(this.ControllerContext.HttpContext, "usrImg", ArrResponse[2]);
                //ViewBag.LoginMessage = "Login Success";
                return RedirectToAction("Index", "WebHome");
            }
            //  String Response = "[{\"Response\":\"" + ObjResponse1.Response + "\"}]";
            // return Json(Response);


            /************************************************************/
            #region Using Json
            /*var _request = JsonConvert.SerializeObject(ObjModel);
            ResponseModel ObjResponse = GetApiResponse(Constant.ApiLoginUser, _request);

            if (String.IsNullOrWhiteSpace(ObjResponse.Response))
            {
                return View("Index", ObjModel);

            }

            var objResponse = ObjResponse.Response;
            ResponseModel ObjResponse1 = JsonConvert.DeserializeObject<ResponseModel>(ObjResponse.Response);
            String VarResponse = ObjResponse1.Response;
            if (VarResponse.Equals("Email/Password is Incorrect"))
            {
                ViewBag.Response = "Email/Password is Incorrect";
                return View("Index", ObjModel);
            }
            else
            {
                string[] ArrResponse = VarResponse.Split(',');
                Services.SetCookie(this.ControllerContext.HttpContext, "usrId", ArrResponse[0]);
                Services.SetCookie(this.ControllerContext.HttpContext, "usrName", ArrResponse[1]);
                Services.SetCookie(this.ControllerContext.HttpContext, "usrImg", ArrResponse[2]);
                //ViewBag.LoginMessage = "Login Success";
                return RedirectToAction("Index", "Home");
            }
            */
            #endregion
            /////////////////////////
        }

        [HttpPost]
        public JsonResult RegisterUser(LoginModel ObjModel)
        {
            String Response = "";

            string res;
            long a;
            string myStr = ObjModel.EmailId;
            res = Int64.TryParse(myStr, out a).ToString();
            if (res == "True")
            {
                ObjModel.Mobile = ObjModel.EmailId;

                if (ObjModel.Mobile.Length != 10)
                {
                    ModelState.AddModelError("EmailId", "Mobile Number Incorrect");
                    Response = "[{\"Response\":\"" + "Mobile Number Incorrect" + "\"}]";
                    return Json(Response);
                }
                ObjModel.EmailId = null;
            }
            else
            {

                bool IsValidEmail = CommonFile.ValidateEmailIsValid(ObjModel.EmailId);
                if (!IsValidEmail)
                {
                    ModelState.AddModelError("EmailId", "Email Incorrect");
                    Response = "[{\"Response\":\"" + "Email Incorrect" + "\"}]";
                    return Json(Response);
                }

                ObjModel.Mobile = null;

            }

            ObjModel.RegisterType = "web";
            string PasswordEncripy = CommonFile.EncodePasswordMd5(ObjModel.Password);

            ObjModel.Password = PasswordEncripy;

            var _request = _JwtTokenManager.GenerateToken(JsonConvert.SerializeObject(ObjModel));
            ResponseModel ObjResponse = CommonFile.GetApiResponseJWT(Constant.ApiRegisterUser, _request);
            ResponseModel ObjResponse1 = JsonConvert.DeserializeObject<ResponseModel>(ObjResponse.Response);
            String varResponse = ObjResponse1.Response;
            if (varResponse.Equals("USER EXISTS"))
            {
                Response = "[{\"Response\":\"" + ObjResponse1.Response + "\"}]";
            }
            else
            {
                string[] ArrResponse = varResponse.Split(',');

                // var jsonString = "{\"Id\":\"" + ArrResponse[0] + "\",\"Name\":\"" + ArrResponse[1] + "\",\"ProfileImg\":\"" + ArrResponse[2] + "\"}";

                var jsonString = "{\"Id\":\"" + ArrResponse[0] + "\",\"Name\":\"" + ArrResponse[1] + "\",\"ProfileImg\":\"" + ArrResponse[2] + "\",\"EmailId\":\"" + ArrResponse[3] + "\",\"Mobile\":\"" + ArrResponse[4] + "\"}";

                Services.SetCookie(this.ControllerContext.HttpContext, "webusr", _JwtTokenManager.GenerateToken(jsonString.ToString()));
                Response = "[{\"Response\":\"" + "Home" + "\"}]"; ;
            }
            return Json(Response);
            /******************************************************************/
            #region Using Json
            /*    var _request = JsonConvert.SerializeObject(ObjModel);
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiRegisterUser, _request);
            if (String.IsNullOrWhiteSpace(ObjResponse.Response))
            {
              //  return View("Index", ObjModel);

            }
            var objResponse = ObjResponse.Response;
            ResponseModel ObjResponse1 = JsonConvert.DeserializeObject<ResponseModel>(ObjResponse.Response);
            ViewBag.RegisterMessage = ObjResponse1.Response;
            String Response = "[{\"Response\":\"" + ObjResponse1.Response + "\"}]";
    */
            #endregion


        }

        [HttpPost]
        public JsonResult ForgetPassword(string emailId)
        {

            String Response = string.Empty;
            bool IsValidEmail = CommonFile.ValidateEmailIsValid(emailId);
            if (!IsValidEmail)
            {
                ModelState.AddModelError("EmailId", "Email Incorrect");
                Response = "[{\"Response\":\"" + "Email Incorrect" + "\"}]";
                return Json(Response);
            }
            LoginModel ObjLoginModel = new LoginModel();
            ObjLoginModel.EmailId = emailId;
            var _request = _JwtTokenManager.GenerateToken(JsonConvert.SerializeObject(ObjLoginModel));
            ResponseModel ObjResponse = CommonFile.GetApiResponseJWT(Constant.ApiIsEmailExist, _request);
            ResponseModel ObjResponse1 = JsonConvert.DeserializeObject<ResponseModel>(ObjResponse.Response);

            string respo = ObjResponse1.Response;
            ViewBag.ResponseMassege = respo;
            if (respo.Equals("Not Exist Email"))
            {
                Response = "[{\"Response\":\"" + respo + "\"}]";
                return Json(Response);
            }

            String subject = "ForgetPassword";
            String Forgetpassword = "";


            #region Using Json
            /*
            var _request = JsonConvert.SerializeObject(ObjModel);
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiForgetPassword, _request);
            String Response = string.Empty;

            if (String.IsNullOrWhiteSpace(ObjResponse.Response))
            {
                Response = "[{\"Response\":\"" + 0 + "\"}]";
                return Json(Response);
            }
            ResponseModel ResponseApi = JsonConvert.DeserializeObject<ResponseModel>(ObjResponse.Response);
            String Forgetpassword = ResponseApi.Response;

            if (Forgetpassword == " 1")
            {
                ViewBag.Response = "Please Check Email ";
            }
            */
            #endregion
            String userName = emailId;
            String Password = Forgetpassword;
            String domainName = Constant.DomainUrl;
            String Path = "Login/ResetPasword/";

            var jsonString = "{\"EmailId\":\"" + emailId + "\",\"Date\":\"" + DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss") + "\"}";
            //  String jwtToken=  _JwtTokenManager.GenerateToken(jsonString.ToString());
            byte[] byt = System.Text.Encoding.UTF8.GetBytes(jsonString);


            // convert the byte array to a Base64 string



            String Base64 = Convert.ToBase64String(byt);
            String ForgetLink = domainName + Path + Base64;


            string body = "Click Here <br/> Reset Password <br/>" + ForgetLink;
            int respoEmail = CommonFile.SendMailContact(emailId, subject, userName, Password, body);
            Response = "[{\"Response\":\"" + respoEmail + "\"}]";
            if (respoEmail == 1)
            {
                ViewBag.Response = "Please Check Email ";
            }
            return Json(Response);
        }
    }
}