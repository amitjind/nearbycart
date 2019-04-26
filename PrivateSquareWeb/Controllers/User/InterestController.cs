using Newtonsoft.Json;
using PrivatesquaresWebApiNew.Models;
using PrivateSquareWeb.CommonCls;
using PrivateSquareWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateSquareWeb.Controllers.User
{
    public class InterestController : Controller
    {
        JwtTokenManager _JwtTokenManager = new JwtTokenManager();
        static List<InterestModel> ListInterest;
        // GET: Interest
        public ActionResult Index()
        {
            var InterestCategoryList = GetInterestCategory();
            ViewBag.InterestCategoryList = new SelectList(InterestCategoryList, "Id", "Name");
            ListInterest = GetAllInterest();
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
        public JsonResult GetCateWiseInterest(long CatId)
        {
            List<InterestModel> FilterInterestModelsList = ListInterest.Where(i => i.InterestCatId == CatId).ToList();
            var JsonResponse = Json(FilterInterestModelsList);
            return JsonResponse;
        }
        public List<InterestModel> GetAllInterest()
        {
            var InterestList = new List<InterestModel>();
           // var _request = "";//_JwtTokenManager.GenerateToken(JsonConvert.SerializeObject(loginModel));
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetAllInterest, "");
            InterestList = JsonConvert.DeserializeObject<List<InterestModel>>(ObjResponse.Response);
            return InterestList;
        }
        [HttpPost]
        public ActionResult SaveInsterest(FormCollection formCollection)
        {
            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);

            
                String SelectedInterest = formCollection["TxtInterest"];
            #region Comment Code For DataSet To Json 
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("UserId");
            dt.Columns.Add("InterestId");
            dt.Columns.Add("InterestCatId");
            String[] Arr_Interest = SelectedInterest.Split(',');
            for (int i = 0; i < Arr_Interest.Length; i++)
            {
                DataRow NewDataRow;
                NewDataRow = dt.NewRow();
                NewDataRow["UserId"] = MdUser.Id;
                NewDataRow["InterestId"] = Arr_Interest[i];
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
            var _request = JSONresult;
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiSaveUserInterest, _request);

            if (String.IsNullOrWhiteSpace(ObjResponse.Response))
            {
                return View("Index");

            }
            return RedirectToAction("GetStart", "Login");
            //return RedirectToAction("Index", "Home"); ;
        }
        //private ResponseModel GetApiResponse(string Url, String Data)
        //{
        //    var _response = Services.GetApiResponseJson(Url, "POST", Data);

        //    ResponseModel _data = JsonConvert.DeserializeObject<ResponseModel>(_response);
        //    ResponseModel ObjResponse = JsonConvert.DeserializeObject<ResponseModel>(_data.Response);
        //    return ObjResponse;
        //}
    }
}