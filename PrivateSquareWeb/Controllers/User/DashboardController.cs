using Newtonsoft.Json;
using PrivatesquaresWebApiNew.Models;
using PrivateSquareWeb.CommonCls;
using PrivateSquareWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateSquareWeb.Controllers.User
{
    public class DashboardController : Controller
    {
        JwtTokenManager _JwtTokenManager = new JwtTokenManager();
        static List<InterestModel> ListInterest;

        // GET: Dashboard
        public ActionResult Index()
        {

            return View();
        }
        public List<InterestCategoryModel> GetInterestCategory()
        {
            var ProductCategoryList = new List<InterestCategoryModel>();
            // var _request = "";//_JwtTokenManager.GenerateToken(JsonConvert.SerializeObject(loginModel));
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetAllInterestCategory, "");
            ProductCategoryList = JsonConvert.DeserializeObject<List<InterestCategoryModel>>(ObjResponse.Response);
            return ProductCategoryList;

        }
            public List<InterestModel> GetAllInterest()
            {
                var InterestList = new List<InterestModel>();
                // var _request = "";//_JwtTokenManager.GenerateToken(JsonConvert.SerializeObject(loginModel));
                ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetAllInterest, "");
                InterestList = JsonConvert.DeserializeObject<List<InterestModel>>(ObjResponse.Response);
                return InterestList;
            }
        public JsonResult GetCateWiseInterest(long CatId)
        {
            List<InterestModel> FilterInterestModelsList = ListInterest.Where(i => i.InterestCatId == CatId).ToList();
            var JsonResponse = Json(FilterInterestModelsList);
            return JsonResponse;
        }
        public List<InterestModel> GetCateWiseInterestEdit()
        {
            UserInterestModel objModel = new UserInterestModel();
            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);

            if (MdUser.Id != 0)
                objModel.UserId = Convert.ToInt64(MdUser.Id);
            var InterestList = new List<InterestModel>();
            var _request = JsonConvert.SerializeObject(objModel);
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetUserInterest, _request);
            InterestList = JsonConvert.DeserializeObject<List<InterestModel>>(ObjResponse.Response);
            return InterestList;

        }
        public List<UserProfileModel> GetUserProfile()
        {

            var GetUserProfile = new List<UserProfileModel>();
            UserProfileModel objUserProfile = new UserProfileModel();
            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);

            if (MdUser.Id != 0)
                objUserProfile.UserId = Convert.ToInt64(MdUser.Id);
            var _request = JsonConvert.SerializeObject(objUserProfile);
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetProfile, _request);
            string jsonResult = ObjResponse.Response;
            GetUserProfile = JsonConvert.DeserializeObject<List<UserProfileModel>>(jsonResult);

            return GetUserProfile;

        }

        public ActionResult PersonalProfile()
        {
            if (!CommonFile.IsUserAuthenticate(this.ControllerContext.HttpContext))
            {
                return RedirectToAction("Index", "Login");
            }
            ListInterest = GetAllInterest();
            var InterestCategoryList = GetInterestCategory();
            ViewBag.InterestCategoryList = new SelectList(InterestCategoryList, "Id", "Name");

            List<InterestCategoryModel> CatwiseInterest = new List<InterestCategoryModel>();
            ViewBag.CatwiseInterestList = new SelectList(CatwiseInterest, "Id", "Name");


            var listProfession = CommonFile.GetProfession();
            ViewBag.ProfessionList = new SelectList(listProfession, "Id", "Name");

            List<InterestModel> ListInterestUser = GetCateWiseInterestEdit();
            ViewBag.ListInterestUser = new SelectList(ListInterestUser, "InterestId", "InterestName");


            bindCountryStateCity();
            UserProfileModel objModel = new UserProfileModel();
            List<UserProfileModel> UserProfile = GetUserProfile();

            if (UserProfile == null || UserProfile.Count() == 0)
            {
                LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);

                if (MdUser.Id != 0)
                {
                    objModel.FirstName = MdUser.Name;
                    objModel.UserId = Convert.ToInt64(MdUser.Id);
                    if (!String.IsNullOrEmpty(MdUser.EmailId))
                    {
                        objModel.EmailId = MdUser.EmailId;
                    }
                    if (!String.IsNullOrWhiteSpace(MdUser.Mobile))
                    {
                        objModel.Phone = MdUser.Mobile;
                    }
                }
                objModel.DOB = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                string[] ArrUserAddress = new string[0];
                ViewBag.UserAddress = ArrUserAddress;
            }
            else if (UserProfile != null && UserProfile.Count() > 0)
            {

                objModel.FirstName = UserProfile[0].FirstName;
                objModel.LastName = UserProfile[0].LastName;
                objModel.Location = UserProfile[0].Location;
                objModel.DOB = UserProfile[0].DOB;
                objModel.ProfessionalCatId = UserProfile[0].ProfessionalCatId;
                objModel.Pincode = UserProfile[0].Pincode;
                objModel.EmailId = UserProfile[0].EmailId;
                objModel.Description = UserProfile[0].Description;
                objModel.Phone = UserProfile[0].Phone;
                objModel.CountryId = UserProfile[0].CountryId;
                objModel.Title = UserProfile[0].Title;
                objModel.ProfessionalKeyword = UserProfile[0].ProfessionalKeyword;
                objModel.ProfileImage = UserProfile[0].ProfileImage;
                objModel.OfficeAddress = UserProfile[0].OfficeAddress;
                objModel.OtherAddress = UserProfile[0].OtherAddress;
                objModel.Phone = UserProfile[0].Phone;
                objModel.InterestCatId = UserProfile[0].InterestCatId;
                objModel.CityId = UserProfile[0].CityId;
                if (!string.IsNullOrWhiteSpace(UserProfile[0].StrUserInterestIds))
                {
                    int[] userInterest = Array.ConvertAll(UserProfile[0].StrUserInterestIds.Split(','), int.Parse);
                    UserProfile[0].UserInterestIds = userInterest;
                    objModel.UserInterestIds = UserProfile[0].UserInterestIds;
                }
                else
                {
                    int[] userInterest = new int[0];
                    UserProfile[0].UserInterestIds = userInterest;
                    objModel.UserInterestIds = UserProfile[0].UserInterestIds;
                }
                if (!string.IsNullOrWhiteSpace(UserProfile[0].StrUserAddress))
                {
                    string[] ArrUserAddress = UserProfile[0].StrUserAddress.Split('^');
                    ViewBag.UserAddress = ArrUserAddress;
                }
                else
                {
                    string[] ArrUserAddress = new string[0];
                    ViewBag.UserAddress = ArrUserAddress;
                }
            }
            else
            {
                string[] ArrUserAddress = new string[0];
                ViewBag.UserAddress = ArrUserAddress;
            }
            return View(objModel);
        }
        public ActionResult MyBusiness()
        {
            if (!CommonFile.IsUserAuthenticate(this.ControllerContext.HttpContext))
            {
                return RedirectToAction("Index", "Login");
            }
            var listProfession = CommonFile.GetProfession();
            ViewBag.ProfessionList = new SelectList(listProfession, "Id", "Name");
            bindCountryStateCity();
            BusinessModel objModel = new BusinessModel();
            return View(objModel);
        }
        public List<BusinessModel> GetBusiness(long Id)
        {
            var GetBusiness = new List<BusinessModel>();
            BusinessModel objUserProfile = new BusinessModel();
            objUserProfile.Id = Id;
            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);

            if (MdUser.Id != 0)
                objUserProfile.UserId = Convert.ToInt64(MdUser.Id);

            var _request = _JwtTokenManager.GenerateToken(JsonConvert.SerializeObject(objUserProfile));
            ResponseModel ObjResponse = CommonFile.GetApiResponseJWT(Constant.ApiGetBusinessDetail, _request);
            GetBusiness = JsonConvert.DeserializeObject<List<BusinessModel>>(ObjResponse.Response);

            return GetBusiness;

        }
        public ActionResult EditMyBusiness(long Id)
        {

            var listProfession = CommonFile.GetProfession();
            ViewBag.ProfessionList = new SelectList(listProfession, "Id", "Name");
            bindCountryStateCity();




            List<BusinessModel> EditBusinessList = GetBusiness(Id);
            BusinessModel objModel = new BusinessModel();
            if (EditBusinessList != null && EditBusinessList.Count() > 0)
            {
                objModel.Id = Id;
                objModel.BusinessName = EditBusinessList[0].BusinessName;
                objModel.BusinessLogo = EditBusinessList[0].BusinessLogo;
                objModel.Location = EditBusinessList[0].Location;
                objModel.ProfessionalCatId = EditBusinessList[0].ProfessionalCatId;
                objModel.ProfessionalKeyword = EditBusinessList[0].ProfessionalKeyword;
                objModel.CityId = EditBusinessList[0].CityId;
                objModel.PinCode = EditBusinessList[0].PinCode;
                objModel.UserId = EditBusinessList[0].UserId;
                objModel.Email = EditBusinessList[0].Email;
                objModel.Description = EditBusinessList[0].Description;
                objModel.CountryId = EditBusinessList[0].CountryId;
                objModel.Phone = EditBusinessList[0].Phone;
                objModel.Website = EditBusinessList[0].Website;
                // objModel.StateId = EditBusinessList[0].StateId;

            }
            return View("MyBusiness", objModel);
        }
        [HttpPost]
        public ActionResult SaveProfile(FormCollection frmColl, UserProfileModel objModel)
        {
            string ServerPath = Server.MapPath("~/DocImg/");
            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            String FileName = null;
            String Address = frmColl["txtAddress"];
            objModel.Location = Address;
            String StrDob = frmColl["DOB"];
            DateTime Dob = DateTime.ParseExact(StrDob, "dd/MM/yyyy", null);
            objModel.DOB = Dob;
            //objModel.ProfessionalKeyword = objModel.ProfessionalKeyword.Substring(0, objModel.ProfessionalKeyword.Length - 1);

            //Add the following lines
            ModelState["DOB"].Errors.Clear();


            if (!string.IsNullOrWhiteSpace(objModel.Location))
                ModelState["Location"].Errors.Clear();

            //UpdateModel(objModel);
            if (ModelState.IsValid)
            {


                HttpPostedFileBase FileUpload = Request.Files["FileUploadImage"];
                FileName = CommonFile.SaveImage(FileUpload,ServerPath);

                objModel.Id = 0;

                if (MdUser.Id != 0)
                {
                    objModel.UserId = Convert.ToInt64(MdUser.Id);
                    // objModel.Phone = MdUser.Mobile;
                }
                objModel.ProfileImage = FileName;

                #region  Code For DataSet To Json 
                DataTable dt = new DataTable();
                dt.Clear();
                dt.Columns.Add("UserId");
                dt.Columns.Add("InterestId");
                dt.Columns.Add("InterestCatId");
                if (objModel.UserInterestIds != null && objModel.UserInterestIds.Length > 0)
                {
                    int[] Arr_Interest = objModel.UserInterestIds;
                    for (int i = 0; i < Arr_Interest.Length; i++)
                    {
                        DataRow NewDataRow;
                        NewDataRow = dt.NewRow();
                        NewDataRow["UserId"] = MdUser.Id;
                        NewDataRow["InterestId"] = Arr_Interest[i];
                        NewDataRow["InterestCatId"] = "0";
                        dt.Rows.Add(NewDataRow);
                    }
                }
                else

                {
                    DataRow NewDataRow;
                    NewDataRow = dt.NewRow();
                    NewDataRow["UserId"] = MdUser.Id;
                    NewDataRow["InterestId"] = 0;
                    NewDataRow["InterestCatId"] = "0";
                    dt.Rows.Add(NewDataRow);
                }
                // Add a Root Object Name
                var collectionWrapper = new
                {
                    Interest = dt
                };
                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(collectionWrapper);
                #endregion
                var data = JsonConvert.DeserializeObject(JSONresult);
                var xmlNode = JsonConvert.DeserializeXmlNode(data.ToString(), "root").OuterXml;
                objModel.XmlData = xmlNode;


                if (!String.IsNullOrEmpty(objModel.Location))
                {
                    String[] ArrayAddress = objModel.Location.Split(',');
                    #region  Code For DataSet To Json For Address 
                    DataTable dtAddress = new DataTable();
                    dtAddress.Clear();
                    dtAddress.Columns.Add("UserId");
                    dtAddress.Columns.Add("Address");
                    //int[] ArrayAddress = objModel.UserInterestIds;
                    for (int i = 0; i < ArrayAddress.Length; i++)
                    {
                        if (!String.IsNullOrWhiteSpace(ArrayAddress[i]))
                        {
                            DataRow NewDataRow;
                            NewDataRow = dtAddress.NewRow();
                            NewDataRow["UserId"] = MdUser.Id;
                            NewDataRow["Address"] = ArrayAddress[i];
                            dtAddress.Rows.Add(NewDataRow);
                        }
                    }
                    // Add a Root Object Name
                    var collectionWrapperAddress = new
                    {
                        Location = dtAddress
                    };
                    string JSONAddressResult;
                    JSONAddressResult = JsonConvert.SerializeObject(collectionWrapperAddress);
                    #endregion
                    #region  Code For DataSet To Xml For Address 
                    var dataAddress = JsonConvert.DeserializeObject(JSONAddressResult);
                    var xmlNodeAddress = JsonConvert.DeserializeXmlNode(dataAddress.ToString(), "root").OuterXml;
                    objModel.XmlDataAddress = xmlNodeAddress;
                    #endregion
                }

                var _request = _JwtTokenManager.GenerateToken(JsonConvert.SerializeObject(objModel));
                ResponseModel ObjResponse = CommonFile.GetApiResponseJWT(Constant.ApiSaveProfile, _request);
                String VarResponse = ObjResponse.Response;
                if (String.IsNullOrWhiteSpace(ObjResponse.Response))
                {
                    return View("Index", objModel);

                }
                string ProfileImgName = "";
                if (!String.IsNullOrEmpty(FileName))
                    ProfileImgName = FileName;
                else
                    ProfileImgName = MdUser.ProfileImg;
                var jsonString = "{\"Id\":\"" + MdUser.Id + "\",\"Name\":\"" + MdUser.Name + "\",\"ProfileImg\":\"" + ProfileImgName + "\",\"EmailId\":\"" + MdUser.EmailId + "\",\"Mobile\":\"" + MdUser.Mobile + "\"}";
                Services.SetCookie(this.ControllerContext.HttpContext, "usr", _JwtTokenManager.GenerateToken(jsonString.ToString()));



                //String UserName = string.Concat(objModel.FirstName, " ", objModel.LastName);
                // Services.SetCookie(this.ControllerContext.HttpContext, "usrName", UserName);
                //HeaderPartialModel objHeaderModel = new HeaderPartialModel();
                //objHeaderModel.UserName = UserName;
                return RedirectToAction("MyBusinessList", "Home");
            }
            else
            {
                ListInterest = GetAllInterest();
                var InterestCategoryList = GetInterestCategory();
                ViewBag.InterestCategoryList = new SelectList(InterestCategoryList, "Id", "Name");

                List<InterestCategoryModel> CatwiseInterest = new List<InterestCategoryModel>();
                ViewBag.CatwiseInterestList = new SelectList(CatwiseInterest, "Id", "Name");


                var listProfession = CommonFile.GetProfession();
                ViewBag.ProfessionList = new SelectList(listProfession, "Id", "Name");

                List<InterestModel> ListInterestUser = GetCateWiseInterestEdit();
                ViewBag.ListInterestUser = new SelectList(ListInterestUser, "InterestId", "InterestName");


                bindCountryStateCity();
                if (!string.IsNullOrWhiteSpace(objModel.Location))
                {
                    string[] ArrUserAddress = objModel.Location.Split(',');
                    ViewBag.UserAddress = ArrUserAddress;
                }
                else
                {
                    ViewBag.UserAddress = "";
                }
                return View("PersonalProfile", objModel);
            }

        }
        [HttpPost]
        public ActionResult SaveBussiness(BusinessModel objModel)
        {
            string ServerPath = Server.MapPath("~/DocImg/");
            var listProfession = CommonFile.GetProfession();
            ViewBag.ProfessionList = new SelectList(listProfession, "Id", "Name");
            bindCountryStateCity();
            if (string.IsNullOrWhiteSpace((objModel.Id).ToString()))
                objModel.Id = 0;

            if (ModelState.IsValid)
            {
                var _requestBusinessCheck = _JwtTokenManager.GenerateToken(JsonConvert.SerializeObject(objModel));
                ResponseModel ObjResponseBusiness = CommonFile.GetApiResponseJWT(Constant.ApiIsBusinessExist, _requestBusinessCheck);
                ResponseModel ObjResponseBusiness1 = JsonConvert.DeserializeObject<ResponseModel>(ObjResponseBusiness.Response);

                string respo = ObjResponseBusiness1.Response;
                ViewBag.ResponseMassege = respo;
                if (respo.Equals("Exist"))
                {
                    ViewBag.ResponseMessage = "Business Name All Ready Exist";
                    // Response = "[{\"Response\":\"" + respo + "\"}]";
                    return View("MyBusiness", objModel);
                }



                HttpPostedFileBase FileUpload = Request.Files["FileUploadImage"];
                String FileName = CommonFile.SaveImage(FileUpload,ServerPath);
                //BusinessModel objModel = new BusinessModel();
                //objModel.Id = 0;
                LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);

                if (MdUser.Id != 0)
                    objModel.UserId = Convert.ToInt64(MdUser.Id);
                objModel.BusinessLogo = FileName;
                if (objModel.Id != 0)
                    objModel.Operation = "update";
                else if (objModel.Id == 0)
                    objModel.Operation = "insert";

                //var _request = JsonConvert.SerializeObject(objModel);
                var _request = _JwtTokenManager.GenerateToken(JsonConvert.SerializeObject(objModel));
                ResponseModel ObjResponse = CommonFile.GetApiResponseJWT(Constant.ApiSaveBusiness, _request);
                String VarResponse = ObjResponse.Response;
                if (String.IsNullOrWhiteSpace(ObjResponse.Response))
                {
                    return View("MyBusiness", objModel);

                }

                return RedirectToAction("MyBusinessList", "Home");
            }
            return View("MyBusiness", objModel);
        }
        private void bindCountryStateCity()
        {
            var CountryList = CommonFile.GetCountry();
            ViewBag.CountryList = new SelectList(CountryList, "Id", "Name");
            var StateList = CommonFile.GetState();
            ViewBag.StateList = new SelectList(StateList, "Id", "Name");
            var CityList = CommonFile.GetCity();
            ViewBag.CityList = new SelectList(CityList, "Id", "Name");

        }
        [HttpPost]
        public JsonResult ShowImages()
        {
            List<ProductImages> ListImagesProduct = new List<ProductImages>();
            int index = 1;
            foreach (string file in Request.Files)
            {
                var fileContent = Request.Files[file];

                if (fileContent != null && fileContent.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file);
                    ProductImages ProImg = new ProductImages();
                    ProImg.Id = index;
                    ProImg.Name = fileName;
                    ProImg.ImageUpload = fileContent;
                    ListImagesProduct.Add(ProImg);
                    index++;

                    // get a stream
                    //var stream = fileContent.InputStream;
                    // and optionally write the file to disk

                    //var path = Path.Combine(Server.MapPath("~/App_Data/Images"), fileName);
                    //using (var fileStream = File.Create(path))
                    //{
                    //    stream.CopyTo(fileStream);
                    //}
                }
            }
            ViewBag.ProImagesList = ListImagesProduct;
            //HttpPostedFileBase FileUpload = Request.Files["FileUploadMultiImage"];

            //foreach (var file in files)
            //{
            //    //Checking file is available to save.  
            //    if (file != null)
            //    {
            //        var InputFileName = Path.GetFileName(file.FileName);
            //        //var ServerSavePath = Path.Combine(Server.MapPath("~/UploadedFiles/") + InputFileName);
            //        ////Save file to server folder  
            //        //file.SaveAs(ServerSavePath);
            //        //assigning file uploaded status to ViewBag for showing message to user.  
            //        ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
            //    }

            //}



            return Json("");
        }

        public JsonResult bindProfessionalKeyword(string Prefix)

        {

            var ProfessionalKeywordList = CommonFile.GetProfessionalKeyword();

            ViewBag.ProfessionalKeywordList = new SelectList(ProfessionalKeywordList, "Id", "Name");

            var EmpName = (from e in ProfessionalKeywordList

                           where e.Name.ToLower().StartsWith(Prefix.ToLower())

                           select new { e.Name, e.Id });

            return Json(EmpName, JsonRequestBehavior.AllowGet);

        }


    }

}