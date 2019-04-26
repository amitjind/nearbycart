using Newtonsoft.Json;
using PrivatesquaresWebApiNew.CommonCls;
using PrivatesquaresWebApiNew.Models;
using PrivatesquaresWebApiNew.Persistance.Repositary;
using PrivatesquaresWebApiNew.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;

namespace PrivatesquaresWebApiNew.Controllers
{
    public class UserController : ApiController
    {
        private IUserServices userServices;

        public UserController()
        {
            userServices = new UserServices(new UserRepositary());
        }
        [Route("api/User/Registeration")]
        [HttpPost]
        // public IHttpActionResult Registeration(RequestModel requestModel)
        public IHttpActionResult Registeration([FromBody] RequestModel requestModel)
        {
            // = JsonConvert.DeserializeObject<ApiRequestModel>(System.Web.HttpContext.Current.Request.Form["encrypted"].ToString());

            //var data = new JwtTokenManager().DecodeToken(requestModel.Data);
            var data = requestModel.Data;
            UserRegisterModel user = JsonConvert.DeserializeObject<UserRegisterModel>(data);
            // if (request.ContainsKey("unique_name"))
            {
                var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.RegisterUser(user)), Success = true };
                var sendJson = Json(sendResponse);
                return sendJson;
            }

            //  return Json(new ResponseModel() { Response = BadRequest().ToString(), Success = false });


        }

        [HttpPost]
        [Route("api/User/AuthenticateUser")]
        public IHttpActionResult AuthenticateUser(RequestModel requestModel)
        {

            // var data = new JwtTokenManager().DecodeToken(requestModel.Data);
            var data = requestModel.Data;
            // Dictionary<string, object> request = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            //if (request.ContainsKey("unique_name"))
            {
                UserRegisterModel login = JsonConvert.DeserializeObject<UserRegisterModel>(data);
                var SendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.Login(login)), Success = true };
                return Json(SendResponse);
            }
            //return Json(new ResponseModel() { Response = BadRequest().ToString(), Success = false });
        }

        [Route("api/User/SaveProfile")]
        [HttpPost]
        // public IHttpActionResult Registeration(RequestModel requestModel)
        public IHttpActionResult SaveUserProfile(RequestModel requestModel)
        {
            #region Comment Code For DataSet To Json 
            //DataTable dt = new DataTable();
            //dt.Clear();
            //dt.Columns.Add("UserId");
            //dt.Columns.Add("InterestId");
            //dt.Columns.Add("InterestCatId");
            //DataRow _ravi;
            //_ravi = dt.NewRow();
            //_ravi["UserId"] = "1";
            //_ravi["InterestId"] = "2";
            //_ravi["InterestCatId"] = "3";
            //dt.Rows.Add(_ravi);
            //_ravi = dt.NewRow();
            //_ravi["UserId"] = "1";
            //_ravi["InterestId"] = "3";
            //_ravi["InterestCatId"] = "4";
            //dt.Rows.Add(_ravi);
            //_ravi = dt.NewRow();
            //_ravi["UserId"] = "1";
            //_ravi["InterestId"] = "4";
            //_ravi["InterestCatId"] = "5";
            //dt.Rows.Add(_ravi);
            //_ravi = dt.NewRow();
            //_ravi["UserId"] = "1";
            //_ravi["InterestId"] = "6";
            //_ravi["InterestCatId"] = "7";
            //dt.Rows.Add(_ravi);

            //string JSONresult;
            //JSONresult = JsonConvert.SerializeObject(dt);
            #endregion

            #region Comment Code For jsonToDataTable
            // List<UserInterestModel> usersList = JsonConvert.DeserializeObject<List<UserInterestModel>>(JSONresult);
            //DataTable dt1= CovnertJsonToDataTable.ToDataTable<UserInterestModel>(usersList);
            #endregion

            var data = new JwtTokenManager().DecodeToken(requestModel.Data);
            Dictionary<string, object> request = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            if (request.ContainsKey("unique_name"))
            {
                UserProfileModel objUserProfile = JsonConvert.DeserializeObject<UserProfileModel>(request["unique_name"].ToString());
                return Json(new ResponseModel() { Response = new JwtTokenManager().GenerateToken(JsonConvert.SerializeObject(userServices.SaveProfile(objUserProfile))), Success = true });
            }
            return Json(new ResponseModel() { Response = BadRequest().ToString(), Success = false });

            #region Using Json Reponse
            /*
            var data = requestModel.Data;
            UserProfileModel objUserProfile = JsonConvert.DeserializeObject<UserProfileModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.SaveProfile(objUserProfile)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
            */
            #endregion
        }
        [Route("api/User/SaveBusiness")]
        [HttpPost]
        // public IHttpActionResult Registeration(RequestModel requestModel)
        public IHttpActionResult SaveBusiness(RequestModel requestModel)
        {

            var data = new JwtTokenManager().DecodeToken(requestModel.Data);
            Dictionary<string, object> request = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            if (request.ContainsKey("unique_name"))
            {
                BusinessModel objBusinessProfile = JsonConvert.DeserializeObject<BusinessModel>(request["unique_name"].ToString());
                return Json(new ResponseModel() { Response = new JwtTokenManager().GenerateToken(JsonConvert.SerializeObject(userServices.SaveBusiness(objBusinessProfile))), Success = true });
            }
            return Json(new ResponseModel() { Response = BadRequest().ToString(), Success = false });




            /*
            var data = requestModel.Data;
            BusinessModel objUserProfile = JsonConvert.DeserializeObject<BusinessModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.SaveBusiness(objUserProfile)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
            */
        }

        [Route("api/User/SaveUserInterest")]
        [HttpPost]

        public IHttpActionResult SaveUserInterest(RequestModel requestModel)
        {

            var data = JsonConvert.DeserializeObject(requestModel.Data);
            var xmlNode = JsonConvert.DeserializeXmlNode(data.ToString(), "root").OuterXml;
            //  XNode node = JsonConvert.DeserializeXNode(data).outerxml;

            UserInterestModel objUserInterest = new UserInterestModel();
            objUserInterest.XmlData = xmlNode;
            // UserInterestModel objUserInterest = JsonConvert.DeserializeObject<UserInterestModel>(data.ToString());
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.SaveUserInterest(objUserInterest)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }

        [Route("api/User/SaveProduct")]
        [HttpPost]
        // public IHttpActionResult Registeration(RequestModel requestModel)
        public IHttpActionResult SaveProduct(RequestModel requestModel)
        {

            var data = requestModel.Data;
            ProductModel objProductModel = JsonConvert.DeserializeObject<ProductModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.SaveProduct(objProductModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }

        [Route("api/User/GetUsersProfile")]
        [HttpPost]
        public IHttpActionResult GetUserProfile(RequestModel requestModel)
        {

            var data = requestModel.Data;
            UsersProfileModel objUserProfileModel = JsonConvert.DeserializeObject<UsersProfileModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetUsersProfile(objUserProfileModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }

        [Route("api/User/GetAllInterest")]
        [HttpPost]
        public IHttpActionResult GetAllInterest(RequestModel requestModel)
        {

            //var data = requestModel.Data;
            //ProductModel objProductModel = JsonConvert.DeserializeObject<ProductModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetAllInterest()), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }
        [Route("api/User/GetAllInterestCategory")]
        [HttpPost]
        public IHttpActionResult GetAllInterestCategory(RequestModel requestModel)
        {

            //var data = requestModel.Data;
            //ProductModel objProductModel = JsonConvert.DeserializeObject<ProductModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetAllInterestCategory()), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }
        [Route("api/User/SaveNetwork")]
        [HttpPost]
        public IHttpActionResult SaveNetwork(RequestModel requestModel)
        {

            var data = requestModel.Data;
            NetworkModel objNetworkModel = JsonConvert.DeserializeObject<NetworkModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.SaveNetwork(objNetworkModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }
        [Route("api/User/GetNetwork")]
        [HttpPost]
        public IHttpActionResult GetNetwork(RequestModel requestModel)
        {

            var data = requestModel.Data;
            NetworkModel objNetworkModel = JsonConvert.DeserializeObject<NetworkModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetNetwork(objNetworkModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }

        [Route("api/User/GetUserBusiness")]
        [HttpPost]
        // public IHttpActionResult Registeration(RequestModel requestModel)
        public IHttpActionResult GetUserBusiness(RequestModel requestModel)
        {

            var data = new JwtTokenManager().DecodeToken(requestModel.Data);
            Dictionary<string, object> request = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            if (request.ContainsKey("unique_name"))
            {
                BusinessModel objProductModel = JsonConvert.DeserializeObject<BusinessModel>(request["unique_name"].ToString());
                return Json(new ResponseModel() { Response = new JwtTokenManager().GenerateToken(JsonConvert.SerializeObject(userServices.GetUserbusiness(objProductModel))), Success = true });
            }
            return Json(new ResponseModel() { Response = BadRequest().ToString(), Success = false });


            //var data = requestModel.Data;
            //BusinessModel objProductModel = JsonConvert.DeserializeObject<BusinessModel>(data);
            //var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetUserbusiness(objProductModel)), Success = true };
            //var sendJson = Json(sendResponse);
            //return sendJson;
        }

        [Route("api/User/GetProfile")]
        [HttpPost]
        // public IHttpActionResult Registeration(RequestModel requestModel)
        public IHttpActionResult GetProfile(RequestModel requestModel)
        {

            var data = requestModel.Data;
            UserProfileModel objProfileModel = JsonConvert.DeserializeObject<UserProfileModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetUserProfile(objProfileModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }

        [Route("api/User/GetBusiness")]

        [HttpPost]

        // public IHttpActionResult Registeration(RequestModel requestModel)

        public IHttpActionResult GetBusiness(RequestModel requestModel)

        {
            var data = requestModel.Data;

            BusinessModel objBusinessModel = JsonConvert.DeserializeObject<BusinessModel>(data);

            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetUserbusiness(objBusinessModel)), Success = true };

            var sendJson = Json(sendResponse);

            return sendJson;

        }
        [Route("api/User/GetProduct")]
        [HttpPost]
        public IHttpActionResult GetProduct(RequestModel requestModel)

        {
            var data = new JwtTokenManager().DecodeToken(requestModel.Data);

            //var data = requestModel.Data;

            ProductModel objProductModel = JsonConvert.DeserializeObject<ProductModel>(data);

            // var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetProduct(objProductModel)), Success = true };
            return Json(new ResponseModel()
            {
                Response = new JwtTokenManager()
                                                            .GenerateToken(
                                                            JsonConvert.SerializeObject(
                                                                userServices.GetProduct(objProductModel))),
                Success = true
            });
            // var sendJson = Json(sendResponse);

            //return sendJson;

        }
        [Route("api/User/GetUserInterest")]
        [HttpPost]
        // public IHttpActionResult Registeration(RequestModel requestModel)

        public IHttpActionResult GetUserInterest(RequestModel requestModel)

        {
            var data = requestModel.Data;

            UserInterestModel objProductModel = JsonConvert.DeserializeObject<UserInterestModel>(data);

            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetUserInterest(objProductModel)), Success = true };

            var sendJson = Json(sendResponse);

            return sendJson;

        }
        [Route("api/User/UpdateUserInterest")]
        [HttpPost]
        public IHttpActionResult UpdateUserInterest(RequestModel requestModel)

        {
            var data = JsonConvert.DeserializeObject(requestModel.Data);

            var xmlNode = JsonConvert.DeserializeXmlNode(data.ToString(), "root").OuterXml;

            //  XNode node = JsonConvert.DeserializeXNode(data).outerxml; 
            UserInterestModel objUserInterest = new UserInterestModel();

            objUserInterest.XmlData = xmlNode;

            // UserInterestModel objUserInterest = JsonConvert.DeserializeObject<UserInterestModel>(data.ToString());

            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.UpdateUserInterest(objUserInterest)), Success = true };

            var sendJson = Json(sendResponse);

            return sendJson;

        }

        [Route("api/User/GetProfession")]
        [HttpPost]
        public IHttpActionResult GetProfession(RequestModel requestModel)
        {
            return Json(new ResponseModel() { Response = new JwtTokenManager().GenerateToken(JsonConvert.SerializeObject(userServices.GetProfession())), Success = true });

            ////var data = requestModel.Data;
            ////ProductModel objProductModel = JsonConvert.DeserializeObject<ProductModel>(data);
            //var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetProfession()), Success = true };
            //var sendJson = Json(sendResponse);
            //return sendJson;
        }

        [Route("api/User/GetCountry")]
        [HttpPost]
        public IHttpActionResult GetCountry(RequestModel requestModel)
        {

            return Json(new ResponseModel() { Response = new JwtTokenManager().GenerateToken(JsonConvert.SerializeObject(userServices.GetCountry())), Success = true });
            #region Comment
            //Json----------------------
            //var data = requestModel.Data;
            //ProductModel objProductModel = JsonConvert.DeserializeObject<ProductModel>(data);
            //var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetCountry()), Success = true };
            //var sendJson = Json(sendResponse);
            // return sendJson;

            // JWT-------------
            //   var data = new JwtTokenManager().DecodeToken(requestModel.Data);
            //  Dictionary<string, object> request = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            // if (request.ContainsKey("unique_name"))
            //{
            // LoginModel objLoginModel = JsonConvert.DeserializeObject<LoginModel>(request["unique_name"].ToString());
            // }
            // return Json(new ResponseModel() { Response = BadRequest().ToString(), Success = false });
            #endregion
        }


        [Route("api/User/GetState")]
        [HttpPost]
        public IHttpActionResult GetState(RequestModel requestModel)
        {
            //var data = requestModel.Data;
            //DropDownModel objDropDownModel = JsonConvert.DeserializeObject<DropDownModel>(data);
            //var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetState(objDropDownModel)), Success = true };
            //var sendJson = Json(sendResponse);
            // return sendJson;


            var data = new JwtTokenManager().DecodeToken(requestModel.Data);
            Dictionary<string, object> request = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            if (request.ContainsKey("unique_name"))
            {
                DropDownModel objDropDownModel = JsonConvert.DeserializeObject<DropDownModel>(request["unique_name"].ToString());
                return Json(new ResponseModel() { Response = new JwtTokenManager().GenerateToken(JsonConvert.SerializeObject(userServices.GetState(objDropDownModel))), Success = true });
            }
            return Json(new ResponseModel() { Response = BadRequest().ToString(), Success = false });
        }

        [Route("api/User/GetProductCategory")]
        [HttpPost]
        // public IHttpActionResult Registeration(RequestModel requestModel)
        public IHttpActionResult GetProductCategory(RequestModel requestModel)
        {
            //return Json(new ResponseModel() { Response = new JwtTokenManager().GenerateToken(JsonConvert.SerializeObject(userServices.GetProductCategory())), Success = true });

            var data = requestModel.Data;
            DropDownModel objDropDownModel = JsonConvert.DeserializeObject<DropDownModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetProductCategory(objDropDownModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }
        [Route("api/User/GetCity")]
        [HttpPost]
        public IHttpActionResult GetCity(RequestModel requestModel)

        {
            //var data = requestModel.Data;
            //DropDownModel objDropDownModel = JsonConvert.DeserializeObject<DropDownModel>(data);
            //var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetCity(objDropDownModel)), Success = true };
            //var sendJson = Json(sendResponse);
            //return sendJson;

            var data = new JwtTokenManager().DecodeToken(requestModel.Data);
            Dictionary<string, object> request = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            if (request.ContainsKey("unique_name"))
            {
                DropDownModel objDropDownModel = JsonConvert.DeserializeObject<DropDownModel>(request["unique_name"].ToString());
                return Json(new ResponseModel() { Response = new JwtTokenManager().GenerateToken(JsonConvert.SerializeObject(userServices.GetCity(objDropDownModel))), Success = true });
            }
            return Json(new ResponseModel() { Response = BadRequest().ToString(), Success = false });

        }

        [Route("api/User/GetProductDetail")]

        [HttpPost]

        public IHttpActionResult GetProductDetail(RequestModel requestModel)

        {
            var data = requestModel.Data;
            ProductModel objProductModel = JsonConvert.DeserializeObject<ProductModel>(data);

            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetProductDetail(objProductModel)), Success = true };

            var sendJson = Json(sendResponse);

            return sendJson;

        }

        [Route("api/User/GetBusinessDetail")]
        [HttpPost]
        public IHttpActionResult GetBusinessDetail(RequestModel requestModel)
        {

            var data = new JwtTokenManager().DecodeToken(requestModel.Data);
            Dictionary<string, object> request = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            if (request.ContainsKey("unique_name"))
            {
                BusinessModel objBusinessProfile = JsonConvert.DeserializeObject<BusinessModel>(request["unique_name"].ToString());
                return Json(new ResponseModel() { Response = new JwtTokenManager().GenerateToken(JsonConvert.SerializeObject(userServices.GetBusinessDetail(objBusinessProfile))), Success = true });
            }
            return Json(new ResponseModel() { Response = BadRequest().ToString(), Success = false });


        }
        [Route("api/User/LoginUser")]

        [HttpPost]
        public IHttpActionResult LoginUser(RequestModel requestModel)
        {
            var data = new JwtTokenManager().DecodeToken(requestModel.Data);
            Dictionary<string, object> request = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            if (request.ContainsKey("unique_name"))
            {
                LoginModel objLoginModel = JsonConvert.DeserializeObject<LoginModel>(request["unique_name"].ToString());
                return Json(new ResponseModel() { Response = new JwtTokenManager().GenerateToken(JsonConvert.SerializeObject(userServices.LoginUser(objLoginModel))), Success = true });
            }
            return Json(new ResponseModel() { Response = BadRequest().ToString(), Success = false });

            /*
            var data = requestModel.Data;
            LoginModel objLoginModel = JsonConvert.DeserializeObject<LoginModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.LoginUser(objLoginModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
            */
        }
        [Route("api/User/RegisterUser")]
        [HttpPost]
        public IHttpActionResult RegisterUser(RequestModel requestModel)
        {
            var data = new JwtTokenManager().DecodeToken(requestModel.Data);
            Dictionary<string, object> request = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            if (request.ContainsKey("unique_name"))
            {
                LoginModel objLoginModel = JsonConvert.DeserializeObject<LoginModel>(request["unique_name"].ToString());
                return Json(new ResponseModel() { Response = new JwtTokenManager().GenerateToken(JsonConvert.SerializeObject(userServices.RegisterNewUser(objLoginModel))), Success = true });
            }
            return Json(new ResponseModel() { Response = BadRequest().ToString(), Success = false });


            /*****************************************************************************/
            #region using Json
            /*
            var data = requestModel.Data;
            LoginModel objLoginModel = JsonConvert.DeserializeObject<LoginModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.RegisterNewUser(objLoginModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
            */
            #endregion
        }
        [Route("api/User/ForgetPassword")]
        [HttpPost]
        public IHttpActionResult ForgetPassword(RequestModel requestModel)
        {
            var data = new JwtTokenManager().DecodeToken(requestModel.Data);
            Dictionary<string, object> request = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            if (request.ContainsKey("unique_name"))
            {
                LoginModel objLoginModel = JsonConvert.DeserializeObject<LoginModel>(request["unique_name"].ToString());
                return Json(new ResponseModel() { Response = new JwtTokenManager().GenerateToken(JsonConvert.SerializeObject(userServices.ForgetPassword(objLoginModel))), Success = true });
            }
            return Json(new ResponseModel() { Response = BadRequest().ToString(), Success = false });


            #region Using Json
            /*
            var data = requestModel.Data;
            LoginModel objLoginModel = JsonConvert.DeserializeObject<LoginModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.ForgetPassword(objLoginModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
            */
            #endregion
        }
        [Route("api/User/ChangePassword")]

        [HttpPost]
        public IHttpActionResult ChangePassword(RequestModel requestModel)
        {
            var data = requestModel.Data;
            LoginModel objLoginModel = JsonConvert.DeserializeObject<LoginModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.ChangePassword(objLoginModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;

        }
        [Route("api/User/SaveContactUs")]

        [HttpPost]
        public IHttpActionResult SaveContactUs(RequestModel requestModel)

        {
            var data = requestModel.Data;

            ContactUsModel objContactUsModel = JsonConvert.DeserializeObject<ContactUsModel>(data);

            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.SaveContactUs(objContactUsModel)), Success = true };

            var sendJson = Json(sendResponse);

            return sendJson;

        }

        [Route("api/User/GetProfessionalKeyword")]
        [HttpPost]
        public IHttpActionResult GetProfessionalKeyword(RequestModel requestModel)

        {
            var data = requestModel.Data;
            DropDownModel objDropDownModel = JsonConvert.DeserializeObject<DropDownModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetProfessionalKeyword(objDropDownModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;

        }

        [Route("api/User/IsEmailExist")]
        [HttpPost]
        public IHttpActionResult IsEmailExist(RequestModel requestModel)
        {
            var data = new JwtTokenManager().DecodeToken(requestModel.Data);
            Dictionary<string, object> request = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            if (request.ContainsKey("unique_name"))
            {
                LoginModel objLoginModel = JsonConvert.DeserializeObject<LoginModel>(request["unique_name"].ToString());
                return Json(new ResponseModel() { Response = new JwtTokenManager().GenerateToken(JsonConvert.SerializeObject(userServices.IsEmailExist(objLoginModel))), Success = true });
            }
            return Json(new ResponseModel() { Response = BadRequest().ToString(), Success = false });



        }

        [Route("api/User/IsBusinessExist")]
        [HttpPost]
        public IHttpActionResult IsBusinessExist(RequestModel requestModel)
        {
            var data = new JwtTokenManager().DecodeToken(requestModel.Data);
            Dictionary<string, object> request = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            if (request.ContainsKey("unique_name"))
            {
                BusinessModel objBusinessModel = JsonConvert.DeserializeObject<BusinessModel>(request["unique_name"].ToString());
                return Json(new ResponseModel() { Response = new JwtTokenManager().GenerateToken(JsonConvert.SerializeObject(userServices.IsBusinessExist(objBusinessModel))), Success = true });
            }
            return Json(new ResponseModel() { Response = BadRequest().ToString(), Success = false });



        }


        [Route("api/User/SaveUserForgetPasswordLink")]
        [HttpPost]
        public IHttpActionResult SaveUserForgetPasswordLink(RequestModel requestModel)
        {
            var data = new JwtTokenManager().DecodeToken(requestModel.Data);
            Dictionary<string, object> request = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            if (request.ContainsKey("unique_name"))
            {
                LoginModel objLoginModel = JsonConvert.DeserializeObject<LoginModel>(request["unique_name"].ToString());
                return Json(new ResponseModel() { Response = new JwtTokenManager().GenerateToken(JsonConvert.SerializeObject(userServices.SaveUserForgetPasswordLink(objLoginModel))), Success = true });
            }
            return Json(new ResponseModel() { Response = BadRequest().ToString(), Success = false });



        }

        [Route("api/User/GetUserAddress")]

        [HttpPost]

        public IHttpActionResult GetUserAddress(RequestModel requestModel)

        {
            var data = requestModel.Data;
            AddressModel objProductModel = JsonConvert.DeserializeObject<AddressModel>(data);

            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetUserAddress(objProductModel)), Success = true };

            var sendJson = Json(sendResponse);

            return sendJson;

        }

        [Route("api/User/SaveAddress")]
        [HttpPost]
        public IHttpActionResult SaveAddress(RequestModel requestModel)

        {
            var data = requestModel.Data;
            AddressModel objAddress = JsonConvert.DeserializeObject<AddressModel>(data);
            //var xmlNode = JsonConvert.DeserializeXmlNode(data.ToString(), "root").OuterXml;
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.SaveAddress(objAddress)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;

        }

        [Route("api/User/SaveOrders")]
        [HttpPost]
        public IHttpActionResult SaveOrders(RequestModel requestModel)

        {

            var data = requestModel.Data;

            SaleOrderModel objSaleOrder = JsonConvert.DeserializeObject<SaleOrderModel>(data);

            //var xmlNode = JsonConvert.DeserializeXmlNode(data.ToString(), "root").OuterXml;

            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.SaveOrders(objSaleOrder)), Success = true };

            var sendJson = Json(sendResponse);

            return sendJson;

        }

        [Route("api/User/SaveAddToCart")]
        [HttpPost]
        public IHttpActionResult SaveAddToCart(RequestModel requestModel)
        {
            var data = requestModel.Data;
            AddToCartModel objAddToCart = JsonConvert.DeserializeObject<AddToCartModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.SaveAddToCart(objAddToCart)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }

        [Route("api/User/GetAddToCart")]
        [HttpPost]
        public IHttpActionResult GetAddToCart(RequestModel requestModel)
        {
            var data = requestModel.Data;
            AddToCartModel objAddToCartModel = JsonConvert.DeserializeObject<AddToCartModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetAddToCart(objAddToCartModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }

        [Route("api/User/GetOrders")]
        [HttpPost]
        public IHttpActionResult GetOrders(RequestModel requestModel)
        {
            var data = requestModel.Data;
            SaleOrderModel objSaleOrderModel = JsonConvert.DeserializeObject<SaleOrderModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetOrders(objSaleOrderModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;

        }

        [Route("api/User/GetPopularProductId")]
        [HttpPost]
        public IHttpActionResult GetPopularProductId(RequestModel requestModel)
        {

            var data = requestModel.Data;
            ProductModel objProductModel = JsonConvert.DeserializeObject<ProductModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetPopularProductId(objProductModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }

        [Route("api/User/GetWishlist")]
        [HttpPost]
        public IHttpActionResult GetWishlist(RequestModel requestModel)
        {
            var data = requestModel.Data;
            AddToCartModel objAddToCartModel = JsonConvert.DeserializeObject<AddToCartModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetWishlist(objAddToCartModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }

        [Route("api/User/SaveWishlist")]
        [HttpPost]
        public IHttpActionResult SaveWishlist(RequestModel requestModel)
        {
            var data = requestModel.Data;
            AddToCartModel objAddToCart = JsonConvert.DeserializeObject<AddToCartModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.SaveWishlist(objAddToCart)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }

        [Route("api/User/GetSortedProducts")]
        [HttpPost]
        public IHttpActionResult GetSortedProducts(RequestModel requestModel)
        {
            var data = requestModel.Data;
            ProductModel objProductModel = JsonConvert.DeserializeObject<ProductModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetSortedProducts(objProductModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }

        [Route("api/User/SaveReview")]
        [HttpPost]
        public IHttpActionResult SaveReview(RequestModel requestModel)
        {
            var data = requestModel.Data;
            ContactUsModel objContactUs = JsonConvert.DeserializeObject<ContactUsModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.SaveReview(objContactUs)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }

        [Route("api/User/GetReview")]
        [HttpPost]
        public IHttpActionResult GetReview(RequestModel requestModel)
        {
            var data = requestModel.Data;
            ContactUsModel objContactUsModel = JsonConvert.DeserializeObject<ContactUsModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetReview(objContactUsModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }

        [Route("api/User/GetCoupon")]
        [HttpPost]
        public IHttpActionResult GetCoupon(RequestModel requestModel)
        {
            var data = requestModel.Data;
            CouponModel objCouponModel = JsonConvert.DeserializeObject<CouponModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetCoupon(objCouponModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }

        [Route("api/User/SaveCouponHistory")]
        [HttpPost]
        public IHttpActionResult SaveCouponHistory(RequestModel requestModel)
        {
            var data = requestModel.Data;
            CouponModel objCouponModel = JsonConvert.DeserializeObject<CouponModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.SaveCouponHistory(objCouponModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }

        [Route("api/User/IsParentCategory")]
        [HttpPost]
        public IHttpActionResult IsParentCategory(RequestModel requestModel)
        {
            var data = requestModel.Data;
            //Dictionary<string, object> request = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            ProductModel objProductModel = JsonConvert.DeserializeObject<ProductModel>(data);
            return Json(new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.IsParentCategory(objProductModel)), Success = true });

        }

        [Route("api/User/GetChildCategory")]
        [HttpPost]
        public IHttpActionResult GetChildCategory(RequestModel requestModel)
        {

            var data = requestModel.Data;
            ProductModel objProductModel = JsonConvert.DeserializeObject<ProductModel>(data);
            var sendResponse = new ResponseModel() { Response = JsonConvert.SerializeObject(userServices.GetChildCategory(objProductModel)), Success = true };
            var sendJson = Json(sendResponse);
            return sendJson;
        }
    }
}
