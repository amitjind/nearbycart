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
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            ViewBag.AllProduct = CommonFile.GetProduct();
            return View();
        }
        //public List<ProductModel> GetProduct()
        //{
        //    var GetProduct = new List<ProductModel>();
        //    ProductModel objProduct = new ProductModel();
        //    //objProduct.UserId = Convert.ToInt64(Services.GetCookie(this.ControllerContext.HttpContext, "usrId").Value);
        //    var _request = JsonConvert.SerializeObject(objProduct);
        //    ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetProduct, _request);
        //    GetProduct = JsonConvert.DeserializeObject<List<ProductModel>>(ObjResponse.Response);
        //    return GetProduct;

        //}
    }
}