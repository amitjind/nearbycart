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

    public class WebHomeController : Controller
    {
        JwtTokenManager _JwtTokenManager = new JwtTokenManager();
        static List<ProductImages> EditProductImageList;
        static List<ProductModel> ListAllProduct;

        // GET: WebHome

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Index()
        {
            if (ListAllProduct == null) {
                ListAllProduct = CommonFile.GetProduct();
            }
            var PopularProducts = CommonFile.GetPopularProduct(0);
            ViewBag.PopularProducts = PopularProducts;
            ViewBag.FirstCategory = PopularProducts.Where(x => x.ParentCatId == long.Parse((Constant.ParentCategories[0]).ToString())).Take(Constant.NumberOfProductsInFrontPage);
            ViewBag.SecondCategory = PopularProducts.Where(x => x.ParentCatId == Constant.ParentCategories[1]).Take(Constant.NumberOfProductsInFrontPage);
            ViewBag.ThirdCategory = PopularProducts.Where(x => x.ParentCatId == Constant.ParentCategories[2]).Take(Constant.NumberOfProductsInFrontPage);
            ViewBag.FourthCategory = PopularProducts.Where(x => x.ParentCatId == Constant.ParentCategories[3]).Take(Constant.NumberOfProductsInFrontPage);
            ViewBag.FifthCategory = PopularProducts.Where(x => x.ParentCatId == Constant.ParentCategories[4]).Take(Constant.NumberOfProductsInFrontPage);
            //ViewBag.FirstCategory = (CommonFile.GetPopularProduct(Constant.ParentCategories[0])).Take(Constant.NumberOfProductsInFrontPage);
            //ViewBag.SecondCategory = (CommonFile.GetPopularProduct(Constant.ParentCategories[1])).Take(Constant.NumberOfProductsInFrontPage);
            //ViewBag.ThirdCategory = (CommonFile.GetPopularProduct(Constant.ParentCategories[2])).Take(Constant.NumberOfProductsInFrontPage);
            //ViewBag.FourthCategory = (CommonFile.GetPopularProduct(Constant.ParentCategories[3])).Take(Constant.NumberOfProductsInFrontPage);
            //ViewBag.FifthCategory = (CommonFile.GetPopularProduct(Constant.ParentCategories[4])).Take(Constant.NumberOfProductsInFrontPage);
            ViewBag.ProductCatList = CommonFile.GetProductCategory(null);
            return View();

        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult ProductDetail(string Id)
        {
            long id=0;
            try
            {
                id = Convert.ToInt64(CommonFile.Decode(Id));
                List<ProductModel> Product = GetProduct(id);
                ProductModel objModel = new ProductModel();
                if (Product != null && Product.Count() > 0)
                {
                    objModel.Id = id;
                    objModel.ProductName = Product[0].ProductName;
                    objModel.ProductCatId = Product[0].ProductCatId;
                    objModel.ProductImage = Product[0].ProductImage;
                    objModel.SellingPrice = Product[0].SellingPrice;
                    objModel.DiscountPrice = Product[0].DiscountPrice;
                    objModel.BusinessId = Product[0].BusinessId;
                    objModel.UserId = Product[0].UserId;
                    objModel.Description = Product[0].Description;
                    objModel.VendorName = Product[0].VendorName;
                    objModel.ProductImages = Product[0].ProductImages;

                    List<ProductImages> ListProductImages = new List<ProductImages>();
                    if (!String.IsNullOrEmpty(objModel.ProductImages))
                    {
                        String[] ProductImages = objModel.ProductImages.Split(',');
                        ListProductImages = GetSelectedProductImages(ProductImages, objModel.ProductImage);
                        EditProductImageList = ListProductImages;
                        ViewBag.ProductImages = ListProductImages;
                    }
                    else
                    {
                        ViewBag.ProductImages = ListProductImages;
                    }
                }
                ViewBag.SimilarProductList = ListAllProduct.Where(x => x.ProductCatId == objModel.ProductCatId).ToList();       //ViewBag for showing similar products in the Product Detail Page
                return View(objModel);
            }
            catch (Exception ex)
            {
                return View("PageNotFound", "Eror");
            }
        
        }
        private List<ProductImages> GetSelectedProductImages(String[] ProductImages, String DefaultImage)
        {
            List<ProductImages> ListProductImages = new List<ProductImages>();


            for (int i = 0; i < ProductImages.Length; i++)
            {
                ProductImages objProductImage = new ProductImages();
                objProductImage.Name = ProductImages[i];
                if (objProductImage.Name.Equals(DefaultImage))
                {
                    objProductImage.IsSelected = true;
                }
                else
                {
                    objProductImage.IsSelected = false;
                }
                ListProductImages.Add(objProductImage);

            }
            return ListProductImages;
        }

        public List<ProductModel> GetProduct(long Id)
        {
            var GetProduct = new List<ProductModel>();
            ProductModel objProduct = new ProductModel();
            objProduct.Id = Id;
            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            var _request = JsonConvert.SerializeObject(objProduct);
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetProductDetail, _request);
            GetProduct = JsonConvert.DeserializeObject<List<ProductModel>>(ObjResponse.Response);
            return GetProduct;

        }


        public JsonResult AddToCart(AddToCartModel objmodel)
        {
            AddToCart objAddToCart = new AddToCart();


            return objAddToCart.AddToCartFun(objmodel, this.ControllerContext.HttpContext);

        }
        [HttpPost]
        
        public JsonResult AddToCartTojquery(AddToCartModel objmodel)
        {
            AddToCart objAddToCart = new AddToCart();            
           JsonResult json= objAddToCart.AddToCartFun(objmodel, this.ControllerContext.HttpContext);           
            return json;
        }
        public JsonResult RemoveQtyToCart(AddToCartModel objmodel)
		{
			AddToCart objAddToCart = new AddToCart();
			return objAddToCart.RemoveQtyToCartFun(objmodel, this.ControllerContext.HttpContext);

		}

		[HttpPost]
		public ActionResult ProcessForm(FormCollection frm, string submit)
		{
			String SearchText = frm["TxtSearch"];
			switch (submit)
			{
				case "Search":
					ViewBag.UsersProduct = SearchProduct(SearchText);
					var ProductCatList = CommonFile.GetProductCategory(null);

					ViewBag.ProductCatList = ProductCatList;
					break;
				case "Cancel":
					ViewBag.Message = "The operation was cancelled!";
					break;
			}
			return View("Index");
		}
		private List<ProductModel> SearchProduct(string ProductName)
		{

			var SearchProductList = ListAllProduct.Where(x => x.ProductName.ToUpper().Contains(ProductName.ToUpper())).ToList();

			return SearchProductList;

		}

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult WishList(ProductModel objmodel)
		{
			LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);
			if (MdUser.Id != 0)
			{
				objmodel.UserId = Convert.ToInt64(MdUser.Id);
			}
			else
            {
                //return JavaScript("window.swal('Please Login to access wishlist');");
                return RedirectToAction("Index", "WebLogin");
            }

			var _request = JsonConvert.SerializeObject(objmodel);
			ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetWishlist, _request);
			var ProductWishlist = JsonConvert.DeserializeObject<List<ProductModel>>(ObjResponse.Response);
			ViewBag.Wishlist = ProductWishlist;
            ViewBag.WishlistCount = ProductWishlist.Count;
            return View(objmodel);
		}

		public ActionResult AddToWishlist(AddToCartModel objmodel)
		{
			LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);
			if (MdUser.Id != 0)
			{ objmodel.UserId = Convert.ToInt64(MdUser.Id); }
			else
			{
				//ViewBag.WishListResponse = "Please Login to access wishlist";
				return Json("Please Login to access wishlist");
			}
			objmodel.Operation = "insert";
			var result = SaveWishlist(objmodel);
			if (result == "Product Exists")
			{
				//ViewBag.WishListResponse = "Product already added to the Wishlist";
				return Json("Product already added to the Wishlist");
			}
			//ViewBag.WishListResponse = "Product added to the Wishlist";
			return Json("Product added to the Wishlist");

		}

		public string SaveWishlist(AddToCartModel objmodel)
		{

			var _request = JsonConvert.SerializeObject(objmodel);
			ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiSaveWishlist, _request);

			return ObjResponse.Response;
		}

		public ActionResult DeleteFromWishlist(int ProductId)
		{
			AddToCartModel objmodel = new AddToCartModel();

			LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);
			if (MdUser.Id != 0)
			{ objmodel.UserId = Convert.ToInt64(MdUser.Id); }
			else
			{
				return View("WebHome");
			}
			objmodel.ProductId = ProductId;
			objmodel.Operation = "delete";
			var _request = JsonConvert.SerializeObject(objmodel);
			ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiSaveWishlist, _request);
			return JavaScript("window.alert(Product removed successfully);");
		}

		public ActionResult SearchBar(HeaderPartialModel objModel)
		{
			ViewBag.LowerLimit = 1;
			ViewBag.PageIndex = 1;
			ViewBag.ProductsFrom = 0;
			//ListAllProduct = CommonFile.GetProduct();
			if (String.IsNullOrWhiteSpace(objModel.SearchBarText))
			{
				//var SearchList = ListAllProduct.Where(x => x.ProductName.ToUpper().Contains(objModel.SearchBarText.ToString().ToUpper())).ToList();
				if (objModel.ParentCatId == 0)
				{
					ViewBag.UsersProduct = ListAllProduct.Take(Constant.NumberOfProducts);
                    ViewBag.ProductsFrom = 1;
                    ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);
					ViewBag.SearchResultCount = ListAllProduct.Count;
					ViewBag.NumberOfPages = 5;
					var jsonString1 = "{\"ParentCatId\":\"" + objModel.ParentCatId + "\",\"SearchBarText\":\"" + objModel.SearchBarText + "\"}";
					Services.SetCookie(this.ControllerContext.HttpContext, "SearchBarCookie", jsonString1.ToString());
					return View();
				}
				var SearchListWithCategory = ListAllProduct.Where(x => x.ParentCatId.Equals(objModel.ParentCatId)).ToList();
				ViewBag.UsersProduct = SearchListWithCategory.Take(Constant.NumberOfProducts);
				//ViewBag.PopularProducts = CommonFile.GetPopularProduct();
				ViewBag.SearchResultCount = SearchListWithCategory.Count;
				ViewBag.NumberOfPages = 5;
				var jsonString2 = "{\"ParentCatId\":\"" + objModel.ParentCatId + "\",\"SearchBarText\":\"" + objModel.SearchBarText + "\"}";
				Services.SetCookie(this.ControllerContext.HttpContext, "SearchBarCookie", jsonString2.ToString());
				return View();
			}
			if (objModel.ParentCatId == 0)
			{
				var SearchList = ListAllProduct.Where(x => x.ProductName.ToUpper().Contains(objModel.SearchBarText.ToString().ToUpper())).ToList();
                ViewBag.ProductsFrom = 1;
                ViewBag.UsersProduct = SearchList.Take(Constant.NumberOfProducts);
                ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);

                if (SearchList.Count == 0) { ViewBag.NoResultFound = "No Result Found"; }
				else { ViewBag.NoResultFound = ""; }
				//ViewBag.PopularProducts = CommonFile.GetPopularProduct();
				ViewBag.SearchResultCount = SearchList.Count;
				if ((SearchList.Count / Constant.NumberOfProducts) < 5)
				{
					if (SearchList.Count % Constant.NumberOfProducts == 0) { ViewBag.NumberOfPages = SearchList.Count / Constant.NumberOfProducts; }
					else { ViewBag.NumberOfPages = (SearchList.Count / Constant.NumberOfProducts) + 1; }
				}
				else
				ViewBag.NumberOfPages = 5;
			}
			else
			{
				var SearchList = ListAllProduct.Where(x => x.ProductName.ToUpper().Contains(objModel.SearchBarText.ToString().ToUpper())).ToList();

				var SearchListWithCategory = SearchList.Where(x => x.ParentCatId.Equals(objModel.ParentCatId)).ToList();

				ViewBag.UsersProduct = SearchListWithCategory.Take(Constant.NumberOfProducts);
				ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);
				//ViewBag.PopularProducts = CommonFile.GetPopularProduct();
				ViewBag.SearchResultCount = SearchListWithCategory.Count;
				ViewBag.NumberOfPages = 5;
			}
			var jsonString = "{\"ParentCatId\":\"" + objModel.ParentCatId + "\",\"SearchBarText\":\"" + objModel.SearchBarText + "\"}";
			Services.SetCookie(this.ControllerContext.HttpContext, "SearchBarCookie", jsonString.ToString());

			int pageindex = ViewBag.PageIndex;
			ViewBag.PageIndex = pageindex;
			var SearchList1 = ListAllProduct.Where(x => x.ProductName.ToUpper().Contains(objModel.SearchBarText.ToString().ToUpper())).ToList();
			//ViewBag.UsersProduct = SearchList1.Skip((pageindex - 1) * 12).Take(12);
			//ViewBag.ProductsFrom = ((pageindex - 1) * 12);
			//ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);
			//ViewBag.SearchResultCount = SearchList1.Count;
			return View();
		}

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult MyOrders(SaleOrderModel objmodel)
		{
			LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);
			if (MdUser.Id != 0)
			{
				objmodel.UserId = Convert.ToInt64(MdUser.Id);
			}
			else
            {
                //return JavaScript("window.alert('Please Log-In');");
                return RedirectToAction("Index", "WebLogin");
            }
			var _request = JsonConvert.SerializeObject(objmodel);
			ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetOrders, _request);
			var Orders = JsonConvert.DeserializeObject<List<SaleOrderModel>>(ObjResponse.Response);

            foreach (var orders in Orders)
            {
                TimeSpan time = new TimeSpan(12, 30, 00);
                DateTime date = orders.OrderDate.Add(time);

                var b = date.ToString("dd-MM-yyyy hh:mm tt");

                orders.OrderDatestring = b;


            }
            ViewBag.MyOrders = Orders;
			return View(objmodel);
		}

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult OrderDetails(string Id)
		{
			long id = Convert.ToInt64(CommonFile.Decode(Id));
			SaleOrderModel objmodel = new SaleOrderModel();
			LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            if (MdUser.EmailId != null)
            {
                if (MdUser.Id != 0)
                {
                    objmodel.UserId = Convert.ToInt64(MdUser.Id);
                }
                objmodel.SaleOrderId = id;
                objmodel.Id = id;
                var _request = JsonConvert.SerializeObject(objmodel);
                ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetOrders, _request);
                var Orderdetails = JsonConvert.DeserializeObject<List<SaleOrderModel>>(ObjResponse.Response);

                decimal TotalAmount = 0;
                foreach (var orders in Orderdetails)
                {
                    TotalAmount = +(orders.Amount * Convert.ToInt32(orders.Quantity)) + (TotalAmount);
                }
                ViewBag.TotalAmount = TotalAmount;
                ViewBag.Orderdetails = Orderdetails;


                return View(objmodel);
            }
            else
            {
                return RedirectToAction("Index", "WebLogin");
            }
		}

		public ActionResult NextPage(string Id)
		{
			long? id = Convert.ToInt64(CommonFile.Decode(Id));
			int pageindex = (int)id;
			if ((pageindex % 5) == 0)
			{
				ViewBag.LowerLimit = (pageindex / 5) * 5;
			}
			else
			{
				ViewBag.LowerLimit = ((pageindex / 5) * 5) + 1;
			}
			ViewBag.PageIndex = pageindex;
			HeaderPartialModel objModel = new HeaderPartialModel();
			string SearchCookieValue = Services.GetCookie(this.HttpContext, "SearchBarCookie").Value;
			dynamic _data = SearchCookieValue;
			var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(_data);
			if (json.ContainsKey("ParentCatId"))
			{
				string ParentCatId = json["ParentCatId"].ToString();
				string SearchBarText = json["SearchBarText"].ToString();
				objModel.ParentCatId = long.Parse(ParentCatId);
				objModel.SearchBarText = SearchBarText;
			}
			#region  For binding products in next page

			//ListAllProduct = CommonFile.GetProduct();
			if (String.IsNullOrWhiteSpace(objModel.SearchBarText))
			{

				if (objModel.ParentCatId == 0)
				{
					ViewBag.UsersProduct = ListAllProduct.Skip((pageindex - 1) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);
					ViewBag.ProductsFrom = ((pageindex - 1) * Constant.NumberOfProducts) + 1;
					ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);
					ViewBag.SearchResultCount = ListAllProduct.Count;
					if ((ViewBag.SearchResultCount / Constant.NumberOfProducts) < (ViewBag.LowerLimit + 4))
					{
						if ((ViewBag.SearchResultCount % Constant.NumberOfProducts) == 0)
						{
							ViewBag.NumberOfPages = (ViewBag.SearchResultCount / Constant.NumberOfProducts);
						}
						else { ViewBag.NumberOfPages = (ViewBag.SearchResultCount / Constant.NumberOfProducts) + 1; }
					}
					else
					{
						ViewBag.NumberOfPages = ViewBag.LowerLimit + 4;
					}
					return View("SearchBar");
				}
				var SearchListWithCategory = ListAllProduct.Where(x => x.ParentCatId.Equals(objModel.ParentCatId)).ToList();
				ViewBag.UsersProduct = SearchListWithCategory.Skip((pageindex - 1) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);
				ViewBag.ProductsFrom = ((pageindex - 1) * Constant.NumberOfProducts) + 1;
				ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);
				ViewBag.SearchResultCount = SearchListWithCategory.Count;
				if ((ViewBag.SearchResultCount / Constant.NumberOfProducts) < (pageindex + 4))
				{
					if ((ViewBag.SearchResultCount % Constant.NumberOfProducts) != 0) { ViewBag.NumberOfPages = (ViewBag.SearchResultCount / Constant.NumberOfProducts) + 1; }
					else { ViewBag.NumberOfPages = ViewBag.SearchResultCount / Constant.NumberOfProducts; }
					//ViewBag.NumberOfPages = ViewBag.LowerLimit + (((ViewBag.SearchResultCount / 10) - 5) - 1);
				}
				else
				{
					ViewBag.NumberOfPages = pageindex + 4;
				}
				return View("SearchBar");
			}
			if (objModel.ParentCatId == 0)
			{
				var SearchList = ListAllProduct.Where(x => x.ProductName.ToUpper().Contains(objModel.SearchBarText.ToString().ToUpper())).ToList();
				ViewBag.UsersProduct = SearchList.Skip((pageindex - 1) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);
				ViewBag.ProductsFrom = ((pageindex - 1) * Constant.NumberOfProducts) + 1;
				ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);
				ViewBag.SearchResultCount = SearchList.Count;
			}
			else
			{
				var SearchList = ListAllProduct.Where(x => x.ProductName.ToUpper().Contains(objModel.SearchBarText.ToString().ToUpper())).ToList();

				var SearchListWithCategory = SearchList.Where(x => x.ParentCatId.Equals(objModel.ParentCatId)).ToList();
				ViewBag.UsersProduct = SearchListWithCategory.Skip((pageindex - 1) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);
				ViewBag.ProductsFrom = ((pageindex - 1) * Constant.NumberOfProducts) + 1;
				ViewBag.ToProductsCount = (Enumerable.Count(ViewBag.UsersProduct) - 1);
				ViewBag.SearchResultCount = SearchListWithCategory.Count;
			}

			#endregion
			if ((ViewBag.SearchResultCount / Constant.NumberOfProducts) < (pageindex + 4))
			{
				if ((ViewBag.SearhResultCount % Constant.NumberOfProducts) != 0) { ViewBag.NumberOfPages = (ViewBag.SearchResultCount / Constant.NumberOfProducts) + 1; }
				else { ViewBag.NumberOfPages = (ViewBag.SearchResultCount / Constant.NumberOfProducts); }
			}
			else
			{
				ViewBag.NumberOfPages = pageindex + 4;
			}
			if (ViewBag.ProductsFrom== ViewBag.SearchResultCount)
			{
				ViewBag.ToProductsCount = ViewBag.ToProductsCount - 1;
			}
			return View("SearchBar");
		}
		public ActionResult MorePages(string Id)
		{
			long? id = Convert.ToInt64(CommonFile.Decode(Id));
			int pageindex = (int)id;
			ViewBag.PageIndex = pageindex;
			HeaderPartialModel objModel = new HeaderPartialModel();
			//HeaderPartialModel objModel = new HeaderPartialModel();
			string SearchCookieValue = Services.GetCookie(this.HttpContext, "SearchBarCookie").Value;
			dynamic _data = SearchCookieValue;
			var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(_data);
			if (json.ContainsKey("ParentCatId"))
			{
				string ParentCatId = json["ParentCatId"].ToString();
				string SearchBarText = json["SearchBarText"].ToString();
				objModel.ParentCatId = long.Parse(ParentCatId);
				objModel.SearchBarText = SearchBarText;
			}
			if (pageindex % 5 == 0)
			{
				ViewBag.LowerLimit = ((pageindex / 5));
			}
			else
			{
				ViewBag.LowerLimit = ((pageindex / 5) * 5) + 1;
			}
			//ViewBag.NumberOfPages = pageindex+4;
			#region  For binding products in next page

			//ListAllProduct = CommonFile.GetProduct();
			if (String.IsNullOrWhiteSpace(objModel.SearchBarText))
			{

				if (objModel.ParentCatId == 0)
				{
					ViewBag.UsersProduct = ListAllProduct.Skip((pageindex - 1) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);
					ViewBag.SearchResultCount = ListAllProduct.Count;
					if ((ViewBag.SearchResultCount / Constant.NumberOfProducts) < (pageindex + 4))
					{
						ViewBag.NumberOfPages = (ViewBag.SearchResultCount / Constant.NumberOfProducts);
					}
					else
					{
						ViewBag.NumberOfPages = ViewBag.LowerLimit + 4;
					}
					ViewBag.ProductsFrom = ((pageindex - 1) * Constant.NumberOfProducts) + 1;
					ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);
					return View("SearchBar");
				}
				var SearchListWithCategory = ListAllProduct.Where(x => x.ParentCatId.Equals(objModel.ParentCatId)).ToList();
				ViewBag.UsersProduct = SearchListWithCategory.Skip((pageindex - 1) * Constant.NumberOfProducts).Take(Constant.NumberOfProducts);
				ViewBag.SearchResultCount = SearchListWithCategory.Count;
				if ((ViewBag.SearchResultCount / Constant.NumberOfProducts) < (pageindex + 4))
				{
					if ((ViewBag.SearhResultCount % Constant.NumberOfProducts) != 0) { ViewBag.NumberOfPages = (ViewBag.SearchResultCount / Constant.NumberOfProducts) + 1; }
					else { ViewBag.NumberOfPages = (ViewBag.SearchResultCount / Constant.NumberOfProducts); }
				}
				else
				{
					ViewBag.NumberOfPages = pageindex + 4;
				}
				ViewBag.ProductsFrom = ((pageindex - 1) * Constant.NumberOfProducts) + 1;
				ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);
				return View("SearchBar");
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
			if ((ViewBag.SearchResultCount / Constant.NumberOfProducts) < (pageindex + 4))
			{
				if ((ViewBag.SearhResultCount % Constant.NumberOfProducts) != 0) { ViewBag.NumberOfPages = (ViewBag.SearchResultCount / Constant.NumberOfProducts) + 1; }
				else { ViewBag.NumberOfPages = (ViewBag.SearchResultCount / Constant.NumberOfProducts); }
			}
			else
			{
				ViewBag.NumberOfPages = pageindex + 4;
			}
			ViewBag.ProductsFrom = ((pageindex - 1) * Constant.NumberOfProducts) + 1;
			ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);
			return View("SearchBar");
		}
		public ActionResult PreviousPages(string Id)
		{
			long? id = Convert.ToInt64(CommonFile.Decode(Id));
			if (id == 0) { id = 1; }
			int pageindex = (int)id;
			ViewBag.PageIndex = pageindex;
			HeaderPartialModel objModel = new HeaderPartialModel();
			if (pageindex % 5 == 0)
			{
				if ((pageindex / 5) == 1)
				{ ViewBag.LowerLimit = ((pageindex / 5)); }
				else
				{
					ViewBag.LowerLimit = pageindex - 5;
				}
			}
			else
			{
				ViewBag.LowerLimit = ((pageindex / 5) * 5) + 1;
			}

			#region  For binding products in next page

			// ListAllProduct = CommonFile.GetProduct();
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
						ViewBag.NumberOfPages = ViewBag.LowerLimit + 5;
					}
					ViewBag.ProductsFrom = ((pageindex - 1) * Constant.NumberOfProducts) + 1;
					ViewBag.ToProductsCount = Enumerable.Count(ViewBag.UsersProduct);
					return View("SearchBar");
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
				return View("SearchBar");
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
			return View("SearchBar");
		}
	}
}