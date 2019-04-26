using Newtonsoft.Json;
using PrivateSquareWeb.CommonCls;
using PrivateSquareWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateSquareWeb.Controllers.Common
{
    public class AddressController : Controller
    {

        JwtTokenManager _JwtTokenManager = new JwtTokenManager();
        // GET: Address

        public ActionResult Index()

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
            return View();

        }
        public PartialViewResult HeaderValue()
        {
            bindCountryStateCity();
            AddressModel objAddress = new AddressModel();
            return PartialView("~/Views/Shared/_AddAddress.cshtml", objAddress);
        }
        public List<AddressModel> GetUserAddress(long? AddressId)

        {

            var GetUserAddressList = new List<AddressModel>();

            AddressModel objmodel = new AddressModel();

            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);

            if (MdUser.Id != 0)

                objmodel.UserId = Convert.ToInt64(MdUser.Id);

            if (AddressId != null)

            {

                objmodel.Id = AddressId;

            }



            var _request = JsonConvert.SerializeObject(objmodel);

            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetUserAddress, _request);

            GetUserAddressList = JsonConvert.DeserializeObject<List<AddressModel>>(ObjResponse.Response);

            return GetUserAddressList;

        }

        public ActionResult RemoveAddress(AddressModel ObjModel, long Id)

        {

            ObjModel.Id = Id;

            if (!CommonFile.IsUserAuthenticate(this.ControllerContext.HttpContext))

            {

                return RedirectToAction("Index", "Login");

            }

            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);

            if (MdUser.Id != 0)

            {

                ObjModel.UserId = Convert.ToInt64(MdUser.Id);

            }

            if (ObjModel.Id != 0)

            {

                ObjModel.Operation = "delete";

            }



            var _request = JsonConvert.SerializeObject(ObjModel);

            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiSaveAddress, _request);

            //ResponseModel ObjResponse1 = JsonConvert.DeserializeObject<ResponseModel>(ObjResponse.Response);





            if (String.IsNullOrWhiteSpace(ObjResponse.Response))

            {

                @ViewBag.ResponseMessage = "Error ! Unable to Submit Address";

                return View("AddAddress", ObjModel);

            }

            if (ObjResponse.Response.Equals("Save Address"))

            {

                //@ViewBag.ResponseMessage = "Your Current Password is Wrong";



                return RedirectToAction("AddressesList", ObjModel);

            }

            //ViewBag.UserAddress = ListUserAddress;

            return RedirectToAction("AddressesList", ObjModel);





        }

        public ActionResult EditAddress(long Id)

        {

            bindCountryStateCity();

            if (!CommonFile.IsUserAuthenticate(this.ControllerContext.HttpContext))

            {

                return RedirectToAction("Index", "Login");

            }

            List<AddressModel> ListAddress = GetUserAddress(Id);

            AddressModel ObjModel = new AddressModel();

            if (ListAddress != null)

            {

                ObjModel.Id = Id;

                ObjModel.Address = ListAddress[0].Address;

                ObjModel.UserId = ListAddress[0].UserId;

                ObjModel.Name = ListAddress[0].Name;

                ObjModel.Mobile = ListAddress[0].Mobile;

                ObjModel.Pincode = ListAddress[0].Pincode;

                ObjModel.Locality = ListAddress[0].Locality;

                ObjModel.CityId = ListAddress[0].CityId;

                ObjModel.StateId = ListAddress[0].StateId;

                ObjModel.Landmark = ListAddress[0].Landmark;

                ObjModel.AlternatePhone = ListAddress[0].AlternatePhone;

                ObjModel.CityName = ListAddress[0].CityName;

                ObjModel.StateName = ListAddress[0].StateName;

            }



            return View("AddAddress", ObjModel);

        }

        public ActionResult AddAddress()

        {

            bindCountryStateCity();

            AddressModel ObjModel = new AddressModel();

            return View(ObjModel);

        }

        public ActionResult AddressesList()

        {

            AddressModel ObjModel = new AddressModel();

            if (!CommonFile.IsUserAuthenticate(this.ControllerContext.HttpContext))

            {

                return RedirectToAction("Index", "Login");

            }

            List<AddressModel> ListUserAddress;

            ListUserAddress = GetUserAddress(null);

            ViewBag.UserAddress = ListUserAddress;





            return View(ObjModel);

        }
        private void bindCountryStateCity()
        {

            //var CountryList = CommonFile.GetCountry();

            //ViewBag.CountryList = new SelectList(CountryList, "Id", "Name");

            var StateList = CommonFile.GetState();

            ViewBag.StateList = new SelectList(StateList, "Id", "Name");

            var CityList = CommonFile.GetCity();

            ViewBag.CityList = new SelectList(CityList, "Id", "Name");



        }

        [HttpPost]
        public ActionResult AddressesList(AddressModel ObjModel)

        {
            List<AddressModel> ListUserAddress;
            ListUserAddress = GetUserAddress(null);
            bindCountryStateCity();
            if (ModelState.IsValid)
            {
                if (!CommonFile.IsUserAuthenticate(this.ControllerContext.HttpContext))
                {
                    return RedirectToAction("Index", "Login");
                }
                LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);
                if (MdUser.Id != 0)
                {
                    ObjModel.UserId = Convert.ToInt64(MdUser.Id);
                }
                if (ObjModel.Id != 0)
                {
                    ObjModel.Operation = "update";
                }
                else
                {
                    ObjModel.Operation = "insert";
                }

                var _request = JsonConvert.SerializeObject(ObjModel);
                ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiSaveAddress, _request);
                if (String.IsNullOrWhiteSpace(ObjResponse.Response))

                {
                    @ViewBag.ResponseMessage = "Error ! Unable to Submit Address";
                    return View("AddAddress", ObjModel);
                }

                if (ObjResponse.Response.Equals("Save Address"))
                {
                    //@ViewBag.ResponseMessage = "Your Current Password is Wrong";
                    ViewBag.UserAddress = ListUserAddress;
                    return RedirectToAction("AddressesList", ObjModel);
                }

                ViewBag.UserAddress = ListUserAddress;
                return RedirectToAction("AddressesList", ObjModel);
            }
            ViewBag.UserAddress = ListUserAddress;
            return View("AddAddress", ObjModel);

        }


        [HttpPost]
        public JsonResult AddressesListJson(AddressModel ObjModel)

        {
            List<AddressModel> ListUserAddress;
            ListUserAddress = GetUserAddress(null);
            bindCountryStateCity();
            string Response = "";
            if (ModelState.IsValid)
            {
                if (!CommonFile.IsUserAuthenticate(this.ControllerContext.HttpContext))
                {
                    Response = "{\"Response\":\"" + "Not Authorize" + "\"}";

                }
                LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);
                if (MdUser.Id != 0)
                {
                    ObjModel.UserId = Convert.ToInt64(MdUser.Id);
                }
                if (ObjModel.Id != 0)
                {
                    ObjModel.Operation = "update";
                }
                else
                {
                    ObjModel.Operation = "insert";
                }

                var _request = JsonConvert.SerializeObject(ObjModel);
                ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiSaveAddress, _request);
                if (String.IsNullOrWhiteSpace(ObjResponse.Response))

                {

                    Response = "{\"Response\":\"" + "Error ! Unable to Submit Address" + "\"}";
                }

                if (ObjResponse.Response.Equals("Save Address"))
                {
                    Response = "{\"Response\":\"" + "Address Save" + "\"}";
                }


            }
            else
            {
                Response = "{\"Response\":\"" + "Please Fill Some Required Filed" + "\"}";
            }
            return Json(Response);

        }
    }
}