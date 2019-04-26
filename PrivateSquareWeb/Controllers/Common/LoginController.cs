using ASPSnippets.FaceBookAPI;
using Newtonsoft.Json;
using PrivateSquareWeb.CommonCls;
using PrivateSquareWeb.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PrivateSquareWeb.Controllers
{
    public class LoginController : Controller
    {
        JwtTokenManager _JwtTokenManager = new JwtTokenManager();
        // GET: Login
        public ActionResult Index()
        {
            Services.RemoveCookie(this.ControllerContext.HttpContext, "usr");
            // Services.RemoveCookie(this.ControllerContext.HttpContext, "usrName");
            //  FaceBookDevelopApiDetail();
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        public ActionResult GetStart()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserRegister(UserRegisterModel ObjModel, FormCollection frmColl)
        {
            String ChkFacebook = frmColl["ChkFacebook"];
            if (ChkFacebook == "on")
            {
                return FacebookLogin();
            }

            if (String.IsNullOrWhiteSpace(ObjModel.Mobile))
                return View("Index");
            // if (ModelState.IsValid)
            {
                String Type = frmColl["txttype"];
                if (Type.Equals("R"))
                    ObjModel.Operation = "register";
                else
                    ObjModel.Operation = "login";

                var _request = JsonConvert.SerializeObject(ObjModel);
                ResponseModel ObjResponse = GetApiResponse(Constant.ApiRegister, _request);
                if (String.IsNullOrWhiteSpace(ObjResponse.Response))
                {
                    return View("Index", ObjModel);

                }
                String Response = ObjResponse.Response;
                String[] arrResponse = Response.Split(',');
                if (arrResponse[0].Equals("EXISTS"))
                {
                    ViewBag.RegisterMessage = "All Ready Exist";
                    return View("Register");

                }
                else
                {
                    Session["OtpData"] = arrResponse[0];
                    Session["LoginType"] = arrResponse[1];
                    Session["Mobile"] = ObjModel.Mobile;
                    ViewBag.OtpMessage = ObjResponse.Response;
                }
                // return View();
                return RedirectToAction("OTP");
            }

        }
        public ActionResult OTP()
        {
            var otp = Session["OtpData"];
            String OtpCheck = (string)otp;
            String Mobile = (string)Session["Mobile"];
            //Session["OtpData"] = OtpCheck;
            //Session["Mobile"] = Mobile;
            UserRegisterModel ObjModel = new UserRegisterModel();
            ViewBag.OtpMessage = OtpCheck;

            //return PartialView("_OTP", ObjModel);
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserRegisterModel ObjModel)
        {
            var otp = Session["OtpData"];
            String OtpCheck = (string)otp;
            if (string.IsNullOrWhiteSpace(ObjModel.Otp))
                return View("OTP", ObjModel);
            if (OtpCheck.Equals(ObjModel.Otp))
            {
                ObjModel.Mobile = (string)Session["Mobile"];
                var _request = JsonConvert.SerializeObject(ObjModel);
                ResponseModel ObjResponse = GetApiResponse(Constant.ApiLogin, _request);

                if (String.IsNullOrWhiteSpace(ObjResponse.Response))
                {
                    return View("Index", ObjModel);

                }
                String VarResponse = ObjResponse.Response;
                string[] ArrResponse = VarResponse.Split(',');
                Services.SetCookie(this.ControllerContext.HttpContext, "usrId", ArrResponse[0]);
                Services.SetCookie(this.ControllerContext.HttpContext, "usrName", ArrResponse[1]);

                //ViewBag.LoginMessage = "Login Success";
                String LoginType = (string)Session["LoginType"];
                if (LoginType.Equals("R"))

                    return RedirectToAction("Index", "Interest");

                else
                    return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewBag.OtpMessage = OtpCheck;
                ViewBag.LoginMessage = "OtpNot";
            }
            return View("OTP");
        }
        private ResponseModel GetApiResponse(string Url, String Data)
        {
            var _response = Services.GetApiResponseJson(Url, "POST", Data);

            ResponseModel _data = JsonConvert.DeserializeObject<ResponseModel>(_response);
            ResponseModel ObjResponse = JsonConvert.DeserializeObject<ResponseModel>(_data.Response);
            return ObjResponse;
        }

        public JsonResult GetOtp(UserRegisterModel ObjModel)
        {
            if (String.IsNullOrWhiteSpace(ObjModel.Mobile))
                return Json("Mobile No Not Blank");
            // if (ModelState.IsValid)
            {
                var _request = JsonConvert.SerializeObject(ObjModel);
                ResponseModel ObjResponse = GetApiResponse(Constant.ApiRegister, _request);
                if (String.IsNullOrWhiteSpace(ObjResponse.Response))
                {
                    return Json("Please Fill Mobile No");

                }
                String Response = "[{\"Response\":\"" + ObjResponse.Response + "\"}]";
                ViewBag.OtpMessage = ObjResponse.Response;
                Session["OtpData"] = ObjResponse.Response;
                return Json(Response);
            }
        }

        [HttpPost]
        public ActionResult LoginUser(LoginModel ObjModel)
        {
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
                    ModelState.AddModelError("EmailId", "Email Incorrect");
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
                if (ArrResponse[5] == "web")
                {
                    ViewBag.Response = "You don't have privillages to access Admin Panel";
                    return View("Index", ObjModel);
                }


                var jsonString = "{\"Id\":\"" + ArrResponse[0] + "\",\"Name\":\"" + ArrResponse[1] + "\",\"ProfileImg\":\"" + ArrResponse[2] + "\",\"EmailId\":\"" + ArrResponse[3] + "\",\"Mobile\":\"" + ArrResponse[4] + "\"}";
                Services.SetCookie(this.ControllerContext.HttpContext, "usr", _JwtTokenManager.GenerateToken(jsonString.ToString()));


                //Services.SetCookie(this.ControllerContext.HttpContext, "usrId", ArrResponse[0]);
                //Services.SetCookie(this.ControllerContext.HttpContext, "usrName", ArrResponse[1]);
                //Services.SetCookie(this.ControllerContext.HttpContext, "usrImg", ArrResponse[2]);
                //ViewBag.LoginMessage = "Login Success";
                return RedirectToAction("Index", "Home");

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

            ObjModel.RegisterType = "mer";
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

                Services.SetCookie(this.ControllerContext.HttpContext, "usr", _JwtTokenManager.GenerateToken(jsonString.ToString()));
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
        private void FaceBookDevelopApiDetail()
        {
            FaceBookConnect.API_Key = "291291764879597";
            FaceBookConnect.API_Secret = "a4148ad27427c346d65bcb456f9d00d9";

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
        }
        [HttpPost]
        public EmptyResult FacebookLogin()
        {
            FaceBookConnect.Authorize("user_photos,email", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, "Home/Index/"));
            return new EmptyResult();
        }
        public ActionResult WebResetPassword(string id)
        {

            String CheckId = id;
            byte[] b = Convert.FromBase64String(id);
            string strOriginal = System.Text.Encoding.UTF8.GetString(b);
            Dictionary<string, string> DictResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(strOriginal);
            String Email = DictResponse["EmailId"];
            String DateTimeLink = DictResponse["Date"];
            String StrLinkId = DictResponse["Id"];
            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// DateTime(); //new DateTime(DateTime.NO)

            // DateTime date1 = DateTime.ParseExact(DateTimeLink, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime date1 = Convert.ToDateTime(DateTimeLink);//, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime date2 = DateTime.ParseExact(CurrentDate, "yyyy-MM-dd HH:mm:ss", null);
            int result = DateTime.Compare(date1, date2);
            TimeSpan diff = date2 - date1;
            double hours = diff.TotalHours;

            if (hours > 6)
            {
                ViewBag.ResponseMassege = "Link has been Expired. Please Try Again..";
            }

            bool IsValied = GetLinkValied(Email, StrLinkId);

            if (!IsValied)
            {
                ViewBag.ResponseMassege = "Link has been Expired. Please Try Again..";
            }


            return View("WebResetPassword");
        }
        [HttpPost]
        public ActionResult WebResetPassword(LoginModel ObjModel, string id)
        {

            if (string.IsNullOrWhiteSpace(ObjModel.Password))
            {
                ModelState.AddModelError("Password", "Password Is Required");
                return View("WebResetPassword", ObjModel);
            }
            if (string.IsNullOrWhiteSpace(ObjModel.ConfirmPassword))
            {
                ModelState.AddModelError("ConfirmPassword", "ConfirmPassword Is Required");
                return View("WebResetPassword", ObjModel);
            }
            String CheckId = id;
            byte[] b = Convert.FromBase64String(id);
            string strOriginal = System.Text.Encoding.UTF8.GetString(b);
            Dictionary<string, string> DictResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(strOriginal);
            String Email = DictResponse["EmailId"];
            String DateTimeLink = DictResponse["Date"];
            String StrLinkId = DictResponse["Id"];
            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// DateTime(); //new DateTime(DateTime.NO)

            //DateTime date1 = DateTime.ParseExact(DateTimeLink, "yyyy-MM-dd HH:mm:ss", null);
            DateTime date1 = Convert.ToDateTime(DateTimeLink);//, "yyyy-MM-dd HH:mm:ss", null);
            DateTime date2 = DateTime.ParseExact(CurrentDate, "yyyy-MM-dd HH:mm:ss", null);
            int result = DateTime.Compare(date1, date2);
            TimeSpan diff = date2 - date1;
            double hours = diff.TotalHours;

            if (hours > 6)
            {
                ViewBag.ResponseMassege = "Link Hasben Expired. Please Try Again..";
                return View("WebResetPassword", ObjModel);
            }
            bool IsValied = GetLinkValied(Email, StrLinkId);

            if (!IsValied)
            {
                ViewBag.ResponseMassege = "Link has been Expired. Please Try Again..";
            }

            ObjModel.EmailId = Email;
            if (ObjModel.Password.Equals(ObjModel.ConfirmPassword))
            {
                string PasswordEncripy = CommonFile.EncodePasswordMd5(ObjModel.Password);
                ObjModel.Password = PasswordEncripy;

                var _request = _JwtTokenManager.GenerateToken(JsonConvert.SerializeObject(ObjModel));
                ResponseModel ObjResponse = CommonFile.GetApiResponseJWT(Constant.ApiForgetPassword, _request);
                ResponseModel ObjResponse1 = JsonConvert.DeserializeObject<ResponseModel>(ObjResponse.Response);

                string respo = ObjResponse1.Response;
                ViewBag.ResponseMassege = respo;
                string Response = "[{\"Response\":\"" + respo + "\"}]";



                // ObjModel.EmailId = Email;
                ObjModel.Operation = "expire";
                var _requestLink = _JwtTokenManager.GenerateToken(JsonConvert.SerializeObject(ObjModel));
                ResponseModel ObjResponseLink = CommonFile.GetApiResponseJWT(Constant.ApiSaveUserForgetPasswordLink, _requestLink);
                ResponseModel ObjResponseLink1 = JsonConvert.DeserializeObject<ResponseModel>(ObjResponseLink.Response);

                string respoLinkId = ObjResponseLink1.Response;
                return View("../WebLogin/Index");
            }
            else
            {
                ViewBag.ResponseMassege = "Passwords don't Match";
                // ModelState.AddModelError("Not Match", "New Password Or ConfirmPassword Not Match");
            }
            return View("WebResetPassword", ObjModel);
        }
        public bool GetLinkValied(String Email, String StrLinkId)
        {
            #region GetLinkId
            LoginModel objModel = new LoginModel();
            objModel.EmailId = Email;
            objModel.Operation = "select";
            var _requestLink = _JwtTokenManager.GenerateToken(JsonConvert.SerializeObject(objModel));
            ResponseModel ObjResponseLink = CommonFile.GetApiResponseJWT(Constant.ApiSaveUserForgetPasswordLink, _requestLink);
            ResponseModel ObjResponseLink1 = JsonConvert.DeserializeObject<ResponseModel>(ObjResponseLink.Response);
            String ResponseApi = "";
            if (String.IsNullOrEmpty(ObjResponseLink1.Response))
                ResponseApi = "0";
            else
                ResponseApi = ObjResponseLink1.Response;
            string respoLinkId = ResponseApi;
            if (!StrLinkId.Equals(respoLinkId))
            {
                return false;
            }
            else
                return true;
            #endregion GetLinkId
        }
    }
}