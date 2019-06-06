using Newtonsoft.Json;
using PrivateSquareWeb.CommonCls;
using PrivateSquareWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PrivateSquareWeb.Controllers.Website
{
    [HandleError]
    public class ProductCatWiseController : Controller
    {
        static List<ProductModel> ListAllProduct;
        static List<ProductModel> sortedproducts;
        JwtTokenManager _JwtTokenManager = new JwtTokenManager();

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Index(string id)
        {
            long? Id = Convert.ToInt64(CommonFile.Decode(id));
            Services.SetCookie(this.ControllerContext.HttpContext, "ProductCatId", id.ToString());
            ListAllProduct = CommonFile.GetProduct();
            ProductModel objmodel = new ProductModel();
            var SearchProductList = new List<ProductModel>();
            if (!CommonFile.IsParentCategory(Id))
            {
                SearchProductList = ListAllProduct.Where(x => x.ProductCatId == Id).ToList();
            }
            else { SearchProductList = ListAllProduct.Where(x => x.ParentCatId == Id).ToList(); }
            ViewBag.SearchCatId = id;
            ViewBag.UsersProduct = SearchProductList;
            ViewBag.SearchResultCount = SearchProductList.Count;
            var ProductCatList = CommonFile.GetProductCategory(null);
            var EncodedCategories = new List<DropDownModel>();
            foreach (var item in ProductCatList)
            {
                DropDownModel dropDown = new DropDownModel();
                dropDown.Name = item.Name;
                dropDown.Id = item.Id;
                dropDown.EncodedId = CommonFile.Encode(item.Id.ToString());
                dropDown.CatIdEncoded = CommonFile.Encode(item.ParentCatId.ToString());
                dropDown.ParentCatId = item.ParentCatId;
                EncodedCategories.Add(dropDown);
            }
            int pageindex = 1;
            ViewBag.UsersProduct = SearchProductList.Skip((pageindex - 1) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);
            ViewBag.LowerLimit = ((pageindex / 5) * 5) + 1;
            ViewBag.PageIndex = pageindex;
            ViewBag.ProductsFrom = ((pageindex - 1) * Constant.NumberOfProducts);
            ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);
            ViewBag.SearchResultCount = SearchProductList.Count;
            ViewBag.LowerLimit = 1;

            int nop = SearchProductList.Count / Constant.NumberOfProducts;
            int remindernop = SearchProductList.Count % 12;

            if (nop < Constant.NumberOfPages)
            {
                if (remindernop > 0)
                {
                    nop = nop + 1;
                }
                ViewBag.NumberOfPages = nop;
            }
            else
            {
                ViewBag.NumberOfPages = Constant.NumberOfPages;
            }

            var ProductCategory = ProductCatList.Where(x => x.Id.Equals(Id));
            ViewBag.ProductCatList = EncodedCategories.Where(x => x.ParentCatId == ProductCategory.Single().ParentCatId);
            if (!CommonFile.IsParentCategory(Id))
            {
                objmodel.ParentCatId = ProductCategory.Single().ParentCatId;
            }
            else
            {
                objmodel.ParentCatId = ProductCategory.Single().Id;
            }
            objmodel.CategoryName = ProductCategory.Single().Name;
            return View(objmodel);
        }
        public ActionResult ProcessForm(FormCollection frm, string submit)
        {
            ProductModel objModel = new ProductModel();
            String SearchText = frm["TxtSearch"];
            String SearchPrice = frm["TxtPriceRange"];
            switch (submit)
            {
                case "Search":
                    ViewBag.UsersProduct = SearchProduct(SearchText);
                    var ProductCatList = CommonFile.GetProductCategory(null);

                    ViewBag.ProductCatList = ProductCatList;
                    break;
                case "PriceRange":
                    ViewBag.UsersProduct = SearchPriceRange(SearchPrice, 0);
                    var ProductCatListPrice = CommonFile.GetProductCategory(null);
                    ViewBag.ProductCatList = ProductCatListPrice;
                    // return PartialCatwiseProductValue(2);
                    break;
            }
            return View("Index");
            // return PartialView("~/Views/ProductCatWise/PartialCatwiseProductValue.cshtml", objModel);
        }
        public PartialViewResult PartialCatwiseProductValue(string id)

        {
            long? Id = Convert.ToInt64(CommonFile.Decode(id));
            var jsonString1 = "{\"ProductCatId\":\"" + Id + "\"}";
            Services.SetCookie(this.ControllerContext.HttpContext, "ProductCatWiseCatId", jsonString1.ToString());
            ProductModel objModel = new ProductModel();
            List<ProductModel> SearchProductList = new List<ProductModel>();
            if (Id == null)
            {
                SearchProductList = ListAllProduct;
            }
            else
            {
                objModel.ProductCatId = Convert.ToInt64(Id);
                if (!CommonFile.IsParentCategory(Id))
                {
                    SearchProductList = ListAllProduct.Where(x => x.ProductCatId == Id).ToList();
                }
                else
                {
                    SearchProductList = ListAllProduct.Where(x => x.ParentCatId == Id).ToList();
                }
            }
            ViewBag.LowerLimit = 1;
            ViewBag.UsersProduct = SearchProductList.Take(12);
            ViewBag.NumberOfPages = SearchProductList.Count / 10;
            ViewBag.SearchResultCount = SearchProductList.Count;
            var ProductCatList = CommonFile.GetProductCategory(Id);
            //var ProductListMdel = new List<DropDownModel>();
            return PartialView("~/Views/ProductCatWise/PartialCatwiseProductValue.cshtml", objModel);
        }
        public PartialViewResult PartialCatwiseProductPrice(decimal Price, String CategoryId)
        {
            long id = Convert.ToInt64(CommonFile.Decode(CategoryId));
            ProductModel objModel = new ProductModel();
            objModel.ProductCatId = id;
            var SearchProductList = SearchPriceRange(Price.ToString(), id);

            ViewBag.UsersProduct = SearchProductList.Take(Constant.NumberOfProducts);
            var ProductCatList = CommonFile.GetProductCategory(id);

            ViewBag.ProductCatList = ProductCatList;
            return PartialView("~/Views/ProductCatWise/PartialCatwiseProductPrice.cshtml", objModel);
        }
        private List<ProductModel> SearchProduct(string ProductName)
        {
            var SearchProductList = ListAllProduct.Where(x => x.ProductName.ToUpper().Contains(ProductName.ToUpper())).ToList();
            return SearchProductList;
        }
        private List<ProductModel> SearchPriceRange(string ProductPrice, long CategoryId)
        {

            var SearchProductList = ListAllProduct.Where(x => x.DiscountPrice > Convert.ToDecimal(0) && (x.DiscountPrice < Convert.ToDecimal(ProductPrice)) && x.ProductCatId == CategoryId).ToList();
            return SearchProductList;
        }

        public ActionResult Sortby(int sortorder, int pageindex, string productcatid)
        {
            long? Id = Convert.ToInt64(CommonFile.Decode(productcatid));
            ProductModel objModel = new ProductModel();
            var SearchProductList = new List<ProductModel>();
            string SortOrder = "";
            switch (sortorder)
            {
                case 1:
                    SortOrder = "DiscountPrice";
                    break;
                case 2:
                    SortOrder = "DiscountPrice desc";
                    break;
                case 3:
                    SortOrder = "Popularity";
                    break;
                case 4:
                    SortOrder = "RecordDate";
                    break;
            }
            sortedproducts = ListAllProduct.Where(x => x.ProductCatId == Id).ToList();
            if (sortedproducts.Count == 0)
            {
                sortedproducts = ListAllProduct.Where(x => x.ParentCatId == Id).ToList();
            }
            if (sortorder == 1)
            {
                sortedproducts = sortedproducts.OrderBy(x => x.DiscountPrice).ToList();
            }
            if (sortorder == 2)
            {
                sortedproducts = sortedproducts.OrderByDescending(x => x.DiscountPrice).ToList();
            }
            ViewBag.UsersProduct = sortedproducts.Take(Constant.NumberOfProducts);
            ViewBag.SearchCatId = productcatid;
            var ProductCatList = CommonFile.GetProductCategory(null);
            ViewBag.ProductsFrom = ((pageindex - 1) * Constant.NumberOfProducts);
            ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);         
            ViewBag.SearchResultCount = sortedproducts.Count;

            int nop = sortedproducts.Count / Constant.NumberOfProducts;
            int remindernop = sortedproducts.Count % 12;
            if (nop < Constant.NumberOfPages)
            {
                if (remindernop > 0)
                {
                    nop = nop + 1;
                }
                ViewBag.NumberOfPages = nop;
            }
            else
            {
                ViewBag.NumberOfPages = Constant.NumberOfPages;
            }
            ViewBag.LowerLimit = 1;


            int NumberOfPages = Convert.ToInt32(ViewBag.NumberOfPages);
            string SearchResultCount = Convert.ToString(sortedproducts.Count);

            Services.SetCookie(this.ControllerContext.HttpContext, "NumberOfPages", NumberOfPages.ToString());
            Services.SetCookie(this.ControllerContext.HttpContext, "LowerLimit", "1");
            Services.SetCookie(this.ControllerContext.HttpContext, "SearchResutlCount", SearchResultCount);


            ViewBag.ProductCatList = ProductCatList;
            return PartialView("~/Views/ProductCatWise/PartialCatwiseProductValue.cshtml", objModel);
        }

        #region -- Pagination methods - obsolete after applying Lazyloader
        public PartialViewResult NextPage(long id, string sortingby)
        {
            string SearchCookieValue = Services.GetCookie(this.HttpContext, "ProductCatWiseCatId").Value;
            dynamic _data = SearchCookieValue;
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(_data);
            int ProductCatId = int.Parse(json["ProductCatId"].ToString());
            int pageindex = (int)id;
            ViewBag.LowerLimit = ((pageindex / 5) * 5) + 1;
            ViewBag.PageIndex = pageindex;

            var SearchProductList = new List<ProductModel>();
            if (!CommonFile.IsParentCategory(ProductCatId))
            {
                if (sortedproducts != null)
                {
                    SearchProductList = sortedproducts;
                }
                else
                {
                    SearchProductList = ListAllProduct.Where(x => x.ProductCatId == ProductCatId).ToList();
                }
            }
            else
            {
                SearchProductList = ListAllProduct.Where(x => x.ParentCatId == ProductCatId).ToList();
            }
            ViewBag.UsersProduct = SearchProductList.Skip((pageindex - 1) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);

            return PartialView("~/Views/ProductCatWise/PartialCatwiseProductValue.cshtml");
        }

        public PartialViewResult MorePages(long id)
        {
            ProductModel objmodel = new ProductModel();
            int pageindex = (int)id;
            ViewBag.PageIndex = pageindex;
            HeaderPartialModel objModel = new HeaderPartialModel();
            if (pageindex % 5 == 0)
            {
                ViewBag.LowerLimit = ((pageindex / 5));
            }
            else
            {
                ViewBag.LowerLimit = ((pageindex / 5) * 5) + 1;
            }

            #region  For binding products in next page


            if (String.IsNullOrWhiteSpace(objModel.SearchBarText))
            {

                if (objModel.ParentCatId == 0)
                {
                    var SearchProductList = new List<ProductModel>();
                    var ProductCatId = Services.GetCookie(this.HttpContext, "ProductCatId").Value;
                    if (!CommonFile.IsParentCategory(long.Parse(CommonFile.Decode(ProductCatId))))
                    {
                        if (sortedproducts != null)
                        {
                            SearchProductList = sortedproducts;
                        }
                        else
                        {
                            SearchProductList = ListAllProduct.Where(x => x.ProductCatId == long.Parse(CommonFile.Decode(ProductCatId))).ToList();
                        }
                    }
                    else { SearchProductList = ListAllProduct.Where(x => x.ParentCatId == long.Parse(CommonFile.Decode(ProductCatId))).ToList(); }
                    ViewBag.UsersProduct = SearchProductList.Skip((pageindex - 1) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);
                    ViewBag.SearchResultCount = SearchProductList.Count;
                    if ((ViewBag.SearchResultCount / Constant.NumberOfProducts) < (pageindex + 4))
                    {
                        if ((ViewBag.SearchResultCount % Constant.NumberOfProducts) != 0) { ViewBag.NumberOfPages = (ViewBag.SearchResultCount / Constant.NumberOfProducts) + 1; }
                        else
                            ViewBag.NumberOfPages = (ViewBag.SearchResultCount / Constant.NumberOfProducts);
                    }
                    else
                    {
                        ViewBag.NumberOfPages = ViewBag.LowerLimit + 4;
                    }
                    ViewBag.ProductsFrom = ((pageindex - 1) * Constant.NumberOfProducts) + 1;
                    string NumberOfPages1 = ViewBag.NumberOfPages.ToString();
                    string LowerLimit1 = ViewBag.LowerLimit.ToString();
                    string SearchResultCount1 = ViewBag.SearchResultCount.ToString();
                    ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);
                    Services.SetCookie(this.ControllerContext.HttpContext, "NumberOfPages", NumberOfPages1);
                    Services.SetCookie(this.ControllerContext.HttpContext, "LowerLimit", LowerLimit1);
                    Services.SetCookie(this.ControllerContext.HttpContext, "SearchResutlCount", SearchResultCount1);
                    return PartialView("~/Views/ProductCatWise/PartialCatwiseProductValue.cshtml");
                }
                var SearchListWithCategory = ListAllProduct.Where(x => x.ParentCatId.Equals(objModel.ParentCatId)).ToList();
                ViewBag.UsersProduct = SearchListWithCategory.Skip((pageindex - 1) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);
                ViewBag.SearchResultCount = SearchListWithCategory.Count;
                if ((ViewBag.SearchResultCount / 10) < (pageindex + 4))
                {
                    ViewBag.NumberOfPages = ViewBag.LowerLimit + ((ViewBag.SearchResultCount - 5) - 1);
                }
                else
                {
                    ViewBag.NumberOfPages = pageindex + 4;
                }
                ViewBag.ProductsFrom = ((pageindex - 1) * Constant.NumberOfProducts) + 1;
                ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);
                string NumberOfPages2 = ViewBag.NumberOfPages;
                string LowerLimit2 = ViewBag.LowerLimit;
                string SearchResultCount2 = ViewBag.SearchResultCount;
                Services.SetCookie(this.ControllerContext.HttpContext, "NumberOfPages", NumberOfPages2);
                Services.SetCookie(this.ControllerContext.HttpContext, "LowerLimit", LowerLimit2);
                Services.SetCookie(this.ControllerContext.HttpContext, "SearchResutlCount", SearchResultCount2);
                return PartialView("~/Views/ProductCatWise/PartialCatwiseProductValue.cshtml");
            }
            if (objModel.ParentCatId == 0)
            {
                var SearchList = ListAllProduct.Where(x => x.ProductName.ToUpper().Contains(objModel.SearchBarText.ToString().ToUpper())).ToList();
                ViewBag.UsersProduct = SearchList.Skip((pageindex - 1) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);
                ViewBag.SearchResultCount = SearchList.Count;
            }
            else
            {
                var SearchList = ListAllProduct.Where(x => x.ProductName.ToUpper().Contains(objModel.SearchBarText.ToString().ToUpper())).ToList();

                var SearchListWithCategory = SearchList.Where(x => x.ParentCatId.Equals(objModel.ParentCatId)).ToList();
                ViewBag.UsersProduct = SearchListWithCategory.Skip((pageindex - 1) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);

                ViewBag.SearchResultCount = SearchListWithCategory.Count;
            }

            #endregion
            if ((ViewBag.SearchResultCount / 10) < (pageindex + 4))
            {
                ViewBag.NumberOfPages = ViewBag.LowerLimit + ((ViewBag.SearchResultCount - 5) - 1);
            }
            else
            {
                ViewBag.NumberOfPages = pageindex + 4;
            }
            ViewBag.ProductsFrom = ((pageindex - 1) * Constant.NumberOfProducts) + 1;
            ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);

            string NumberOfPages3 = ViewBag.NumberOfPages;
            string LowerLimit3 = ViewBag.LowerLimit;
            string SearchResultCount3 = ViewBag.SearchResultCount;
            Services.SetCookie(this.ControllerContext.HttpContext, "NumberOfPages", NumberOfPages3);
            Services.SetCookie(this.ControllerContext.HttpContext, "LowerLimit", LowerLimit3);
            Services.SetCookie(this.ControllerContext.HttpContext, "SearchResutlCount", SearchResultCount3);
            return PartialView("~/Views/ProductCatWise/PartialCatwiseProductValue.cshtml");
        }

        public PartialViewResult PreviousPages(long id)
        {

            int pageindex = (int)id;
            //if (pageindex != 0)
            //{
            ViewBag.PageIndex = pageindex;
            HeaderPartialModel objModel = new HeaderPartialModel();

            ViewBag.LowerLimit = id - 4;
            #region  For binding products in next page


            if (String.IsNullOrWhiteSpace(objModel.SearchBarText))
            {

                if (objModel.ParentCatId == 0)
                {
                    ViewBag.UsersProduct = ListAllProduct.Skip((pageindex - 1) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);
                    ViewBag.SearchResultCount = ListAllProduct.Count;
                    if ((ViewBag.SearchResultCount / 10) < (pageindex + 4))
                    {
                        ViewBag.NumberOfPages = ViewBag.LowerLimit * 5;
                    }
                    else
                    {
                        ViewBag.NumberOfPages = pageindex;
                    }
                    ViewBag.ProductsFrom = ((pageindex - 1) * Constant.NumberOfProducts) + 1;
                    ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);

                    Services.SetCookie(this.ControllerContext.HttpContext, "NumberOfPages", ViewBag.NumberOfPages.ToString());
                    Services.SetCookie(this.ControllerContext.HttpContext, "LowerLimit", ViewBag.LowerLimit.ToString());
                    Services.SetCookie(this.ControllerContext.HttpContext, "SearchResutlCount", ViewBag.SearchResultCount.ToString());
                    return PartialView("~/Views/ProductCatWise/PartialCatwiseProductValue.cshtml");
                }
                var SearchListWithCategory = ListAllProduct.Where(x => x.ParentCatId.Equals(objModel.ParentCatId)).ToList();
                ViewBag.UsersProduct = SearchListWithCategory.Skip((pageindex - 1) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);
                ViewBag.SearchResultCount = SearchListWithCategory.Count;
                if ((ViewBag.SearchResultCount / 10) < (pageindex + 4))
                {
                    ViewBag.NumberOfPages = ViewBag.LowerLimit + ((ViewBag.SearchResultCount - 5) - 1);
                }
                else
                {
                    ViewBag.NumberOfPages = pageindex + 4;
                }
                ViewBag.ProductsFrom = ((pageindex - 1) * Constant.NumberOfProducts) + 1;
                ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);
                string NumberOfPages2 = ViewBag.NumberOfPages;
                string LowerLimit2 = ViewBag.LowerLimit;
                string SearchResultCount2 = ViewBag.SearchResultCount;
                Services.SetCookie(this.ControllerContext.HttpContext, "NumberOfPages", NumberOfPages2);
                Services.SetCookie(this.ControllerContext.HttpContext, "LowerLimit", LowerLimit2);
                Services.SetCookie(this.ControllerContext.HttpContext, "SearchResutlCount", SearchResultCount2);
                return PartialView("~/Views/ProductCatWise/PartialCatwiseProductValue.cshtml");
            }
            if (objModel.ParentCatId == 0)
            {
                var SearchList = ListAllProduct.Where(x => x.ProductName.ToUpper().Contains(objModel.SearchBarText.ToString().ToUpper())).ToList();
                ViewBag.UsersProduct = SearchList.Skip((pageindex - 1) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);
                ViewBag.SearchResultCount = SearchList.Count;
            }
            else
            {
                var SearchList = ListAllProduct.Where(x => x.ProductName.ToUpper().Contains(objModel.SearchBarText.ToString().ToUpper())).ToList();

                var SearchListWithCategory = SearchList.Where(x => x.ParentCatId.Equals(objModel.ParentCatId)).ToList();
                ViewBag.UsersProduct = SearchListWithCategory.Skip((pageindex - 1) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);

                ViewBag.SearchResultCount = SearchListWithCategory.Count;
            }

            #endregion
            ViewBag.ProductsFrom = ((pageindex - 1) * Constant.NumberOfProducts) + 1;
            ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);
            string NumberOfPages3 = ViewBag.NumberOfPages;
            string LowerLimit3 = ViewBag.LowerLimit;
            string SearchResultCount3 = ViewBag.SearchResultCount;
            Services.SetCookie(this.ControllerContext.HttpContext, "NumberOfPages", NumberOfPages3);
            Services.SetCookie(this.ControllerContext.HttpContext, "LowerLimit", LowerLimit3);
            Services.SetCookie(this.ControllerContext.HttpContext, "SearchResutlCount", SearchResultCount3);
            return PartialView("~/Views/ProductCatWise/PartialCatwiseProductValue.cshtml");
            //}
            //else
            //{

            //}
        }

        public PartialViewResult _Pagination(long id, long? searchresultcount, long? lowerlimit, long? numberofpages)
        {
            ProductModel ObjModel = new ProductModel();

            {
                searchresultcount = long.Parse(Services.GetCookie(this.HttpContext, "SearchResutlCount").Value);
                lowerlimit = long.Parse(Services.GetCookie(this.HttpContext, "LowerLimit").Value);
                numberofpages = long.Parse(Services.GetCookie(this.HttpContext, "NumberOfPages").Value);
            }

            ViewBag.LowerLimit = lowerlimit;
            ViewBag.SearchResultCount = searchresultcount;
            ViewBag.NumberOfPages = numberofpages;

            return PartialView();
        }

        public PartialViewResult PageCountingPartialView(long id)
        {
            string SearchCookieValue = Services.GetCookie(this.HttpContext, "ProductCatWiseCatId").Value;
            dynamic _data = SearchCookieValue;
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(_data);
            int ProductCatId = int.Parse(json["ProductCatId"].ToString());
            int pageindex = (int)id;
            ViewBag.LowerLimit = ((pageindex / 5) * 5) + 1;
            ViewBag.PageIndex = pageindex;

            var SearchProductList = new List<ProductModel>();
            if (!CommonFile.IsParentCategory(ProductCatId))
            {
                SearchProductList = ListAllProduct.Where(x => x.ProductCatId == ProductCatId).ToList();
            }
            else
            {
                SearchProductList = ListAllProduct.Where(x => x.ParentCatId == ProductCatId).ToList();
            }
            ViewBag.UsersProduct = SearchProductList.Skip((pageindex - 1) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);
            ViewBag.ProductsFrom = ((pageindex - 1) * Constant.NumberOfProducts) + 1;

            ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);
            ViewBag.SearchResultCount = SearchProductList.Count;
            ViewBag.NumberOfPages = SearchProductList.Count / 10;
            return PartialView("~/Views/ProductCatWise/PageCountingPartialView.cshtml");
        }
        #endregion

        public PartialViewResult LazyLoader(int id, string sortingby)
        {
            string SearchCookieValue = Services.GetCookie(this.HttpContext, "ProductCatWiseCatId").Value;
            dynamic _data = SearchCookieValue;
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(_data);
            int ProductCatId = int.Parse(json["ProductCatId"].ToString());
            int pageindex = id;
            var SearchProductList = new List<ProductModel>();
            if (!CommonFile.IsParentCategory(ProductCatId))
            {
                if (sortingby == "1")
                {
                    SearchProductList = ListAllProduct.Where(x => x.ProductCatId == ProductCatId).OrderBy(x => x.DiscountPrice).ToList();
                }
                if (sortingby == "2")
                {
                    SearchProductList = ListAllProduct.Where(x => x.ProductCatId == ProductCatId).OrderByDescending(x => x.DiscountPrice).ToList();
                }
                if (sortingby == "3")
                {
                    SearchProductList = ListAllProduct.Where(x => x.ProductCatId == ProductCatId).OrderByDescending(x => x.Id).ToList();
                }
                if (sortingby == "")
                    SearchProductList = ListAllProduct.Where(x => x.ProductCatId == ProductCatId).ToList();
            }
            else
            {
                if (sortingby == "1")
                {
                    SearchProductList = ListAllProduct.Where(x => x.ParentCatId == ProductCatId).OrderBy(x => x.DiscountPrice).ToList();
                }
                if (sortingby == "2")
                {
                    SearchProductList = ListAllProduct.Where(x => x.ParentCatId == ProductCatId).OrderByDescending(x => x.DiscountPrice).ToList();
                }
                if (sortingby == "3")
                {
                    SearchProductList = ListAllProduct.Where(x => x.ParentCatId == ProductCatId).OrderByDescending(x => x.Id).ToList();
                }
                if(sortingby=="")
                    SearchProductList = ListAllProduct.Where(x => x.ParentCatId == ProductCatId).ToList();
            }
            ViewBag.LazyLoaderProducts = SearchProductList.Skip((pageindex) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);

            return PartialView("~/Views/Shared/LazyLoaderProducts.cshtml");
        }
    }
}
