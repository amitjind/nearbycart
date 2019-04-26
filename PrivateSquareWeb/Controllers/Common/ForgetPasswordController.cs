using Newtonsoft.Json;
using PrivateSquareWeb.CommonCls;
using PrivateSquareWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateSquareWeb.Controllers
{
    public class ForgetPasswordController : Controller
    {
        JwtTokenManager _JwtTokenManager = new JwtTokenManager();
        // GET: ForgetPassword
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(LoginModel objModel)
        {
            if (String.IsNullOrWhiteSpace(objModel.EmailId))
            {
                ModelState.AddModelError("EmailId", "Email Is Required");
                return View("Index", objModel);
            }
            String Response = string.Empty;
            bool IsValidEmail = CommonFile.ValidateEmailIsValid(objModel.EmailId);
            if (!IsValidEmail)
            {
                ModelState.AddModelError("EmailId", "Email Incorrect");

                //Response = "[{\"Response\":\"" + "Email Incorrect" + "\"}]";
                return View("Index", objModel);
            }
            //LoginModel ObjLoginModel = new LoginModel();
            //ObjLoginModel.EmailId = emailId;
            var _request = _JwtTokenManager.GenerateToken(JsonConvert.SerializeObject(objModel));
            ResponseModel ObjResponse = CommonFile.GetApiResponseJWT(Constant.ApiIsEmailExist, _request);
            ResponseModel ObjResponse1 = JsonConvert.DeserializeObject<ResponseModel>(ObjResponse.Response);

            string respo = ObjResponse1.Response;
            ViewBag.ResponseMassege = respo;
            if (respo.Equals("Not Exist Email"))
            {
                ViewBag.ResponseMessage = "We couldn't find your account with that information";
                // Response = "[{\"Response\":\"" + respo + "\"}]";
                return View("Index", objModel);
            }
            #region GetLinkId
            objModel.Operation = "insert";
            var _requestLink = _JwtTokenManager.GenerateToken(JsonConvert.SerializeObject(objModel));
            ResponseModel ObjResponseLink = CommonFile.GetApiResponseJWT(Constant.ApiSaveUserForgetPasswordLink, _requestLink);
            ResponseModel ObjResponseLink1 = JsonConvert.DeserializeObject<ResponseModel>(ObjResponseLink.Response);

            string respoLinkId = ObjResponseLink1.Response;
            #endregion
            String subject = "ForgetPassword";
            String Forgetpassword = "";

            String userName = objModel.EmailId;
            String Password = Forgetpassword;
            String domainName = Constant.DomainUrl;
            String Path = "Login/ResetPasword/";

            var jsonString = "{\"EmailId\":\"" + objModel.EmailId + "\",\"Date\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +" \",\"Id\":\""+ respoLinkId.ToString()+"\"}";
            //  String jwtToken=  _JwtTokenManager.GenerateToken(jsonString.ToString());
            byte[] byt = System.Text.Encoding.UTF8.GetBytes(jsonString);


            // convert the byte array to a Base64 string
            String Base64 = Convert.ToBase64String(byt);
            String ForgetLink = domainName + Path + Base64;
            string body = "Click Here <br/> Reset Password <br/>" + ForgetLink;
            int respoEmail = CommonFile.SendMailContact(objModel.EmailId, subject, userName, Password, body);
            // Response = "[{\"Response\":\"" + respoEmail + "\"}]";
            if (respoEmail == 1)
            {
                ViewBag.ResponseMessage = "Please check your email and click the secure link.";
            }
            return View("Index", objModel);
            //return Json(Response);
        }
    }
}