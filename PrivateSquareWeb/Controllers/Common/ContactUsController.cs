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
    public class ContactUsController : Controller
    {
        // GET: ContactUs
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ContactUs(ContactUsModel ObjModel)
        {
            String Response = string.Empty;
            if (ModelState.IsValid)

            {
                string body = "Thanks For Submit Request";
                string emailId = ObjModel.Email;
                string subject = "Contact Us";
                string userName = ObjModel.FullName;
                string Password = "";
                var _request = JsonConvert.SerializeObject(ObjModel);
                ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiSaveContactUs, _request);
                int respo = CommonFile.SendMailContact(emailId, subject, userName, Password, body);
                Response = "[{\"Response\":\"" + respo + "\"}]";
                if (String.IsNullOrWhiteSpace(ObjResponse.Response))
                {
                    
                    return View("Index", ObjModel);
                }
                ViewBag.ResponseMessage = "Your Request has been submit";
                return View("Index");

            }

            return View("Index", ObjModel);

        }
    }
}