using Newtonsoft.Json;
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
    public class ProductsController : Controller
    {
        JwtTokenManager _JwtTokenManager = new JwtTokenManager();

        static List<ProductImages> EditProductImageList;
        // GET: Products
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProductList()
        {
            //if (!CommonFile.IsUserAuthenticate(this.ControllerContext.HttpContext))
            //{
            //    return RedirectToAction("Index", "Login");
            //}
            ViewBag.UsersProduct = CommonFile.GetProduct();
            return View();
        }
        //public List<ProductModel> GetProduct()
        //{
        //    var GetUserProductList = new List<ProductModel>();
        //    ProductModel objmodel = new ProductModel();
        //    LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);
        //    if (MdUser.Id != 0)
        //        objmodel.UserId = Convert.ToInt64(MdUser.Id);
        //    var _request = JsonConvert.SerializeObject(objmodel);
        //    ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetProduct, _request);
        //    GetUserProductList = JsonConvert.DeserializeObject<List<ProductModel>>(ObjResponse.Response);
        //    return GetUserProductList;

        //}
        public ActionResult Product()
        {
            if (!CommonFile.IsUserAuthenticate(this.ControllerContext.HttpContext))
            {
                return RedirectToAction("Index", "Login");
            }

            preRequistProduct(0);
            ProductModel objModel = new ProductModel();
            ViewBag.ProImagesList = "";
            return View(objModel);
        }
        private void preRequistProduct(long editId)
        {
            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            long UserId = 0;
            if (MdUser.Id != 0)
                UserId = MdUser.Id;
            var ProductCatList = CommonFile.GetProductCategory(null);
            ViewBag.ProductCatList = new SelectList(ProductCatList, "Id", "Name");
            var BusinessList = CommonFile.GetUsersBusiness(UserId);
            ViewBag.BusinessList = new SelectList(BusinessList, "Id", "BusinessName");
            List<ProductImages> ListProductImages = new List<ProductImages>();
            ViewBag.ProductImages = ListProductImages;
            if (editId == 0)
                EditProductImageList = null;
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
        [HttpPost]
        public ActionResult SaveProduct(FormCollection frmColl, ProductModel ObjProductModel, string radioBtn, string radioBtnEdit)
        {

            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);

            // objModel.XmlData = xmlNode;
            //var YourRadioButton1 = Request.Form["SetDefaultImage1"];
            // String Imag= Request.Form["img[]"].ToString();


            if (ModelState.IsValid)
            {
                HttpPostedFileBase FileUpload = Request.Files["FileUploadImage"];
                string ServerPath = Server.MapPath("~/DocImg/");
                String FileName = CommonFile.SaveImage(FileUpload, ServerPath);

                ObjProductModel.ProductImage = FileName;
                ObjProductModel.XmlProductImage = GetProductImageXml(MdUser.Id, radioBtn, radioBtnEdit);

                if (MdUser.Id != 0)
                    ObjProductModel.UserId = Convert.ToInt64(MdUser.Id);

                if (ObjProductModel.Id == 0)
                    ObjProductModel.Operation = "insert";
                else if (ObjProductModel.Id != 0)
                    ObjProductModel.Operation = "Update";

                var _request = JsonConvert.SerializeObject(ObjProductModel);
                ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiSaveProduct, _request);
                if (String.IsNullOrWhiteSpace(ObjResponse.Response))
                {
                    return View("Product", ObjProductModel);

                }
                return RedirectToAction("ProductList", "Products");
            }
            else
            {

                List<ProductImages> ListProductImages = new List<ProductImages>();
                if (ObjProductModel.Id != 0)
                {

                    ViewBag.ProductImages = EditProductImageList;
                }
                else
                {
                    ViewBag.ProImagesList = "";
                }
                preRequistProduct(ObjProductModel.Id);
            }
            return View("Product", ObjProductModel);
        }

        public List<ProductModel> GetProduct(long Id)
        {
            var GetProduct = new List<ProductModel>();
            ProductModel objProduct = new ProductModel();
            objProduct.Id = Id;
            LoginModel MdUser = Services.GetLoginUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            if (MdUser.Id != 0)
                objProduct.UserId = Convert.ToInt64(MdUser.Id);
            var _request = JsonConvert.SerializeObject(objProduct);
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetProductDetail, _request);
            GetProduct = JsonConvert.DeserializeObject<List<ProductModel>>(ObjResponse.Response);
            return GetProduct;

        }
        public ActionResult EditProduct(long Id)
        {
            if (!CommonFile.IsUserAuthenticate(this.ControllerContext.HttpContext))
            {
                return RedirectToAction("Index", "Login");
            }
            //var ProductCatList = CommonFile.GetProductCategory();
            //ViewBag.ProductCatList = new SelectList(ProductCatList, "Id", "Name");
            preRequistProduct(Id);
            List<ProductModel> Product = GetProduct(Id);
            ProductModel objModel = new ProductModel();
            if (Product != null && Product.Count() > 0)
            {
                objModel.Id = Id;
                objModel.ProductName = Product[0].ProductName;
                objModel.ProductCatId = Product[0].ProductCatId;
                objModel.ProductImage = Product[0].ProductImage;
                objModel.SellingPrice = Product[0].SellingPrice;
                objModel.DiscountPrice = Product[0].DiscountPrice;
                objModel.BusinessId = Product[0].BusinessId;
                objModel.UserId = Product[0].UserId;
                objModel.Description = Product[0].Description;
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
                    // String[] ProductImages = new String [0];
                    ViewBag.ProductImages = ListProductImages;
                }
            }
            return View("Product", objModel);
        }
        private String GetProductImageXml(long UserId, string radioBtn, string radioBtnEdit)
        {
            string ServerPath = Server.MapPath("~/DocImg/");
            #region
            string radioId = "radioBtn";
            //foreach (string file in Request.Files)
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("UserId");
            dt.Columns.Add("ImageName");
            dt.Columns.Add("IsDefault");
            for (int i = 0; i < Request.Files.Count; i++)
            {
                if (i != 0)
                {
                    var fileContent = Request.Files[i];
                    if (fileContent == null && fileContent.ContentLength == 0)
                    {
                        break;
                    }
                    String ConradioId = radioId + i.ToString();
                    var RadioButton = Request.Form[ConradioId];

                    DataRow NewDataRow;
                    NewDataRow = dt.NewRow();
                    NewDataRow["UserId"] = UserId;
                    //if (RadioButton != null)
                    //{
                    //    NewDataRow["IsDefault"] = "1";
                    //    String SelectedImage = i.ToString();
                    //}
                    if (!String.IsNullOrWhiteSpace(radioBtn))
                    {
                        if (i == Convert.ToInt32(radioBtn))
                        {
                            NewDataRow["IsDefault"] = "1";
                            String SelectedImage = i.ToString();
                        }
                    }
                    else
                    {
                        NewDataRow["IsDefault"] = "0";
                    }

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        var fileName = CommonFile.SaveImage(fileContent, ServerPath);
                        NewDataRow["ImageName"] = fileName;
                    }
                    dt.Rows.Add(NewDataRow);
                }
            }
            ///////////////////// Using For Get Edit Value
            if (EditProductImageList != null && EditProductImageList.Count > 0)
            {
                for (int i = 0; i < EditProductImageList.Count; i++)
                {
                    DataRow NewDataRow;
                    NewDataRow = dt.NewRow();
                    NewDataRow["UserId"] = UserId;
                    if (!String.IsNullOrWhiteSpace(radioBtnEdit))
                    {
                        if (i == Convert.ToInt32(radioBtnEdit))
                        {
                            NewDataRow["IsDefault"] = "1";
                            String SelectedImage = i.ToString();
                        }
                    }
                    else
                    {
                        NewDataRow["IsDefault"] = "0";
                    }
                    var fileName = EditProductImageList[i].Name;
                    NewDataRow["ImageName"] = fileName;

                    dt.Rows.Add(NewDataRow);
                }
            }
            ////////////////////////
            var collectionWrapper = new
            {
                ProductImages = dt
            };
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(collectionWrapper);
            #endregion
            var data = JsonConvert.DeserializeObject(JSONresult);
            var xmlNode = JsonConvert.DeserializeXmlNode(data.ToString(), "root").OuterXml;
            return xmlNode;
        }
        private String EditGetProductImageXml(long UserId, string radioBtn)
        {
            string ServerPath = Server.MapPath("~/DocImg/");
            #region
            string radioId = "radioBtn";
            //foreach (string file in Request.Files)
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("UserId");
            dt.Columns.Add("ImageName");
            dt.Columns.Add("IsDefault");
            for (int i = 0; i < Request.Files.Count; i++)
            {
                if (i != 0)
                {

                    String ConradioId = radioId + i.ToString();
                    var RadioButton = Request.Form[ConradioId];

                    DataRow NewDataRow;
                    NewDataRow = dt.NewRow();
                    NewDataRow["UserId"] = UserId;
                    if (RadioButton != null)
                    {
                        NewDataRow["IsDefault"] = "1";
                        String SelectedImage = i.ToString();
                    }
                    else
                    {
                        NewDataRow["IsDefault"] = "0";
                    }
                    var fileContent = Request.Files[i];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        var fileName = CommonFile.SaveImage(fileContent, ServerPath);
                        NewDataRow["ImageName"] = fileName;
                    }
                    dt.Rows.Add(NewDataRow);
                }
            }
            var collectionWrapper = new
            {
                ProductImages = dt
            };
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(collectionWrapper);
            #endregion
            var data = JsonConvert.DeserializeObject(JSONresult);
            var xmlNode = JsonConvert.DeserializeXmlNode(data.ToString(), "root").OuterXml;
            return xmlNode;
        }
        public JsonResult AddImageList(FormCollection formColl)
        {
            string[] imageUrlFc = formColl["ImageUrl"].Split(',');
            string[] ImageName = formColl["ImageName"].Split(','); ;
            var ImageFile = Request.Files;
            List<ProductImages> ListImagesProduct = new List<ProductImages>();
            for (int i = 0; i < imageUrlFc.Length; i++)
            {
                var fileContent = Request.Files[i];
                ProductImages ProImg = new ProductImages();
                ProImg.Id = i;
                ProImg.Name = ImageName[i];
                ProImg.ImageUrl = imageUrlFc[i];
                // ProImg.ImageUpload = fileContent; 

                ListImagesProduct.Add(ProImg);
            }
            /* List<ProductImages> ListImagesProduct = new List<ProductImages>();
             int index = 1;
             var FileUpload = Request.Files["files"];

             //foreach (var file in files)
             //{
             foreach (string file in Request.Files)
             {
                 var fileContent = Request.Files[file];

                 if (fileContent != null && fileContent.ContentLength > 0)
                 {
                     var fileName = Path.GetFileName(fileContent.FileName);
                     ProductImages ProImg = new ProductImages();
                     ProImg.Id = index;
                     ProImg.Name = fileName;
                     ProImg.ImageUpload = fileContent;
                     ProImg.ImageUrl = imageUrlFc;
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

     */
            var jsonResult = Json(ListImagesProduct, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpPost]
        public JsonResult EditDeleteImage(int RemoveIndex)
        {
            EditProductImageList.RemoveAt(RemoveIndex);
            var jsonResult = Json(EditProductImageList);
            return jsonResult;
        }
        public ActionResult ProductDetail(long id)
        {
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
                    // String[] ProductImages = new String [0];
                    ViewBag.ProductImages = ListProductImages;
                }
            }

            return View(objModel);
        }

    }
    
}