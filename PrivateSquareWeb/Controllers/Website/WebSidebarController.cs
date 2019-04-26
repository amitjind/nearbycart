using PrivateSquareWeb.CommonCls;
using PrivateSquareWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateSquareWeb.Controllers.Website
{
    public class WebSidebarController : Controller

    {
       
        // GET: WebSidebar
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult WebSiteSidebar()
        {
            ProductModel objModel = new ProductModel();
            var ProductCatList = CommonFile.GetProductCategory(null);
            ViewBag.ProductCatList = ProductCatList;
            //ViewBag.ProdCatList36 = ProductCatList.Where(x => x.ParentCatId == 36).ToList();
            var ProdCatList36 = ProductCatList.Where(x => x.ParentCatId == 36).ToList();
                var ProductListMdel = new List<DropDownModel>();
                foreach (var item in ProdCatList36)
                {
                    DropDownModel objmodel = new DropDownModel();
                    var encoded = CommonFile.Encode(item.Id.ToString());
                    objmodel.Name = item.Name;
                    objmodel.Id = item.Id;
                    objmodel.EncodedId =encoded;
                    objmodel.ParentCatId = item.ParentCatId;
                    ProductListMdel.Add(objmodel);
                }
                ViewBag.ProdCatList36 = ProductListMdel;
            //ViewBag.ProdCatList37 = ProductCatList.Where(x => x.ParentCatId == 37).ToList();
            var ProdCatList37 = ProductCatList.Where(x => x.ParentCatId == 37).ToList();
             ProductListMdel = new List<DropDownModel>();
            foreach (var item in ProdCatList37)
            {
                DropDownModel objmodel = new DropDownModel();
                var encoded =CommonFile.Encode(item.Id.ToString());
                objmodel.Name = item.Name;
                objmodel.Id = item.Id;
                objmodel.EncodedId = encoded;
                objmodel.ParentCatId = item.ParentCatId;
                ProductListMdel.Add(objmodel);
            }
            ViewBag.ProdCatList37 = ProductListMdel;
            //ViewBag.ProdCatList38 = ProductCatList.Where(x => x.ParentCatId == 38).ToList();
            var ProdCatList38 = ProductCatList.Where(x => x.ParentCatId == 38).ToList();
            ProductListMdel = new List<DropDownModel>();
            foreach (var item in ProdCatList38)
            {
                DropDownModel objmodel = new DropDownModel();
                var encoded =CommonFile.Encode(item.Id.ToString());
                objmodel.Name = item.Name;
                objmodel.Id = item.Id;
                objmodel.EncodedId = encoded;
                objmodel.ParentCatId = item.ParentCatId;
                ProductListMdel.Add(objmodel);
            }
            ViewBag.ProdCatList38 = ProductListMdel;
            //ViewBag.ProdCatList39 = ProductCatList.Where(x => x.ParentCatId == 39).ToList();
            var ProdCatList39 = ProductCatList.Where(x => x.ParentCatId == 39).ToList();
            ProductListMdel = new List<DropDownModel>();
            foreach (var item in ProdCatList39)
            {
                DropDownModel objmodel = new DropDownModel();
                var encoded =CommonFile.Encode(item.Id.ToString());
                objmodel.Name = item.Name;
                objmodel.Id = item.Id;
                objmodel.EncodedId = encoded;
                objmodel.ParentCatId = item.ParentCatId;
                ProductListMdel.Add(objmodel);
            }
            ViewBag.ProdCatList39 = ProductListMdel;
            //ViewBag.ProdCatList41 = ProductCatList.Where(x => x.ParentCatId == 41).ToList();
            var ProdCatList41 = ProductCatList.Where(x => x.ParentCatId == 41).ToList();
            ProductListMdel = new List<DropDownModel>();
            foreach (var item in ProdCatList41)
            {
                DropDownModel objmodel = new DropDownModel();
                var encoded =CommonFile.Encode(item.Id.ToString());
                objmodel.Name = item.Name;
                objmodel.Id = item.Id;
                objmodel.EncodedId = encoded;
                objmodel.ParentCatId = item.ParentCatId;
                ProductListMdel.Add(objmodel);
            }
            ViewBag.ProdCatList41 = ProductListMdel;
            //ViewBag.ProdCatLiskt42 = ProductCatList.Where(x => x.ParentCatId == 42).ToList();
            var ProdCatList42 = ProductCatList.Where(x => x.ParentCatId == 42).ToList();
            ProductListMdel = new List<DropDownModel>();
            foreach (var item in ProdCatList42)
            {
                DropDownModel objmodel = new DropDownModel();
                var encoded =CommonFile.Encode(item.Id.ToString());
                objmodel.Name = item.Name;
                objmodel.Id = item.Id;
                objmodel.EncodedId = encoded;
                objmodel.ParentCatId = item.ParentCatId;
                ProductListMdel.Add(objmodel);
            }
            ViewBag.ProdCatList42 = ProductListMdel;
            return PartialView("~/Views/Shared/_WebSiteSidebar.cshtml");
        }
       
    }
}