﻿using Newtonsoft.Json;
using PrivateSquareWeb.CommonCls;
using PrivateSquareWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
namespace PrivateSquareWeb.Controllers.Website
{
    public class CheckoutController : Controller
    {
        JwtTokenManager _JwtTokenManager = new JwtTokenManager();
        public ActionResult Index()
        {
            LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            if (MdUser.Id == 0)
                return RedirectToAction("Index", "WebLogin");
            else
            {
                PreRequiestCheckout();
            }
            return View(BindCartItems());
        }
        public List<AddressModel> GetUserAddress()
        {
            AddressModel objmodel = new AddressModel();
            LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            if (MdUser.Id != 0)
                objmodel.UserId = Convert.ToInt64(MdUser.Id);
            var _request = JsonConvert.SerializeObject(objmodel);
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetUserAddress, _request);
            return JsonConvert.DeserializeObject<List<AddressModel>>(ObjResponse.Response).ToList();
        }
        [HttpPost]
        public JsonResult GetAddress(long? id)
        {
            AddressModel objmodel = new AddressModel();
            LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            if (MdUser.Id != 0)
                objmodel.UserId = Convert.ToInt64(MdUser.Id);
            objmodel.Id = id;
            var _request = JsonConvert.SerializeObject(objmodel);
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetUserAddress, _request);
            return Json(ObjResponse.Response);
        }
        private void bindCountryStateCity()
        {
            var CityList = CommonFile.GetCity();
            ViewBag.CityList = new SelectList(CityList, "Id", "Name");
        }
        [HttpPost]
        public ActionResult AddressesList(AddressModel ObjModel)
        {
            LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            ObjModel.UserId = MdUser.Id;
            if (ObjModel.Id == null)
                ObjModel.Id = 0;
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
            if (string.IsNullOrWhiteSpace(ObjResponse.Response))
            {
                ViewBag.ResponseMessage = "Error ! Unable to Submit Address";
                PreRequiestCheckout();
                return View("AddAddress", ObjModel);
            }
            if (ObjResponse.Response.Equals("Save Address"))
            {
                ViewBag.UserAddress = GetUserAddress();
                PreRequiestCheckout();
                AddressModel ObjModelnew = new AddressModel();
                ObjModel = null;
                BindCartItems();
                return RedirectToAction("Index", "Checkout", ObjModel);
            }
            else
            {
                ObjModel = null;
                BindCartItems();
                return RedirectToAction("Index", "Checkout", ObjModel);
            }
        }
        private void PreRequiestCheckout()
        {
            LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            AddToCart objAddToCart = new AddToCart();
            ViewBag.LoginEmail = MdUser.EmailId;
            ViewBag.RegisterType = MdUser.RegisterType;
            ViewBag.ItemCount = objAddToCart.GetItemCount(this.ControllerContext.HttpContext);
            ViewBag.TotalAmount = objAddToCart.GetTotalAmountCheckOut(this.ControllerContext.HttpContext);
            if (ViewBag.TotalAmount < Constant.MinimumAmountForFreeDelivery)
            {
                ViewBag.ShippingCharges = Constant.ShippingCharges;                       //setting shipping/delivery charges
                ViewBag.TotalAmountAfterCharges = ViewBag.TotalAmount + ViewBag.ShippingCharges;
            }
            else
            {
                ViewBag.ShippingCharges = 0;
                ViewBag.TotalAmountAfterCharges = ViewBag.TotalAmount;
            }
            ViewBag.UserAddress = GetUserAddress();
            bindCountryStateCity();
        }
        [HttpPost]
        public JsonResult PlaceOrder(long AddressId, string PaymentMode)
        {
            LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            SaleOrderModel objModel = new SaleOrderModel();
            List<AddToCartModel> ListAddToCart = Services.GetMyCart(this.ControllerContext.HttpContext, _JwtTokenManager);

            objModel = GetSaleOrderValues(ListAddToCart, PaymentMode);
            objModel.Operation = "insert";
            objModel.PaymentMode = PaymentMode;
            objModel.UserId = MdUser.Id;
            var _request = JsonConvert.SerializeObject(objModel);
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiSaveOrders, _request);
            if (ObjResponse.Response.Equals("Order Saved"))
            {
                var _newrequest = JsonConvert.SerializeObject(objModel);
                var Orders = CommonFile.GetApiResponse(Constant.ApiGetOrders, _newrequest);
                Services.RemoveCookie(this.ControllerContext.HttpContext, "addtocart");
            }
            String Response = "[{\"Response\":\"" + ObjResponse.Response + "\"}]";
            return Json(Response);
        }
        private SaleOrderModel GetSaleOrderValues(List<AddToCartModel> ListCart, string PaymentMode)
        {
            decimal TotalAmount = 0;
            decimal TotalDiscount = 0;
            for (int i = 0; i < ListCart.Count; i++)
            {
                TotalAmount += ListCart[i].Amount;
                TotalDiscount += ListCart[i].Discount;
            }
            SaleOrderModel objModel = new SaleOrderModel();
            if (PaymentMode != "Pick Up Order")
            {
                if (TotalAmount <= 500) { TotalAmount += 50; }
            }
            objModel.TotalAmount = TotalAmount;
            objModel.TotalDiscount = TotalDiscount;
            objModel.XmlSaleOrderDetail = ListToXml(ListCart);
            return objModel;
        }
        private String ListToXml(List<AddToCartModel> ListCart)
        {
            LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);

            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("UserId");
            dt.Columns.Add("ProductId");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Discount");
            dt.Columns.Add("Amount");

            for (int i = 0; i < ListCart.Count; i++)
            {
                DataRow NewDataRow;
                NewDataRow = dt.NewRow();
                NewDataRow["UserId"] = MdUser.Id;
                NewDataRow["ProductId"] = ListCart[i].ProductId;
                NewDataRow["Quantity"] = ListCart[i].Qty;
                NewDataRow["Discount"] = ListCart[i].Discount;
                NewDataRow["Amount"] = ListCart[i].Amount;
                dt.Rows.Add(NewDataRow);
            }
            var collectionWrapperAddress = new
            {
                SaleOrderDetail = dt
            };
            string JSONAddressResult;
            JSONAddressResult = JsonConvert.SerializeObject(collectionWrapperAddress);
            #region  Code For DataSet To Xml For Address 
            var dataAddress = JsonConvert.DeserializeObject(JSONAddressResult);
            var xmlNodeSaleOrder = JsonConvert.DeserializeXmlNode(dataAddress.ToString(), "root").OuterXml;

            #endregion
            return xmlNodeSaleOrder;
        }
        public bool iscouponapplied;
        [HttpPost]
        public ActionResult GetCoupon(string CouponCode)
        {
            CouponModel ObjCouponModel = new CouponModel();
            LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            if (MdUser.Id != 0)
                ObjCouponModel.UserId = Convert.ToInt64(MdUser.Id);
            ObjCouponModel.CouponCode = CouponCode;
            var _request = JsonConvert.SerializeObject(ObjCouponModel);
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetCoupon, _request);
            var ObjResponse1 = JsonConvert.DeserializeObject<List<ResponseModel>>(ObjResponse.Response);
            var couponresponse = ObjResponse1[0].Response.ToString();
            if (couponresponse.Equals("already used"))
            {
                ViewBag.CouponResponse = "Coupon already used";
                return JavaScript("swal('coupon already used')");
            }
            if (couponresponse.Equals("coupon expired"))
            {
                ViewBag.CouponResponse = "coupon expired";
                return JavaScript("swal('Coupon Expired')");
            }
            if (couponresponse.Equals("coupon does not exist"))
            {
                ViewBag.CouponResponse = "coupon does not exist";
                return JavaScript("swal('Coupon does not exist')");
            }
            else
            {
                if (iscouponapplied == false)
                {
                    string[] ArrResponse = couponresponse.Split(',');
                    if (ArrResponse[3] == "Discount")                   //if coupon type is "discount"
                    {
                        ViewBag.CouponResponse = ArrResponse[6];
                        AddToCart objAddToCart = new AddToCart();
                        var totalamount = objAddToCart.GetTotalAmountCheckOut(this.ControllerContext.HttpContext);
                        if (totalamount < Convert.ToInt64(ArrResponse[4]))
                        {
                            return JavaScript("swal('Cart value is not sufficient')");
                        }
                        ViewBag.TotalAmount = totalamount - Convert.ToInt64(ArrResponse[2]);
                        if (ViewBag.TotalAmount <= Constant.MinimumAmountForFreeDelivery)
                        {
                            ViewBag.TotalAmount += Constant.ShippingCharges;                          //Adding shipping/delivery charges
                            ViewBag.TotalAmount -= (2 * (ViewBag.TotalAmount));  //making the total negative for handling it securely in client side
                            ViewBag.TotalAmountAfterCharges = ViewBag.TotalAmount;
                        }
                        totalamount = ViewBag.TotalAmount;
                        SaleOrderModel objmodel = new SaleOrderModel();
                        objmodel.TotalAmount = totalamount;
                        ObjCouponModel.IsCouponApplied = true;
                        iscouponapplied = true;
                        objmodel.IsCouponApplied = true;
                        return Json(ViewBag.TotalAmount);
                    }
                    else if (ArrResponse[3] == "BOGO")        //Condition for Coupon Type=="BOGO" (Buy One Get One)
                    {
                        return JavaScript("swal('Coupon does not exist')");
                    }
                    return JavaScript("swal('Error')");
                }
                return JavaScript("swal('Error')");
            }
        }
        #region Save Coupon History
        //private void SaveCouponHistory(SaleOrderModel couponModel)
        //{
        //    CouponModel ObjcouponModel = new CouponModel();
        //    LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);
        //    if (MdUser.Id != 0)
        //    {
        //        ObjcouponModel.UserId = MdUser.Id;
        //    }
        //    ObjcouponModel.CouponId = 1;
        //    ObjcouponModel.OrderId = 15;
        //    ObjcouponModel.Discount = 100;
        //    ObjcouponModel.Operation = "insert";
        //    var _request = JsonConvert.SerializeObject(ObjcouponModel);
        //    ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiSaveCouponHistory, _request);
        //    if (ObjResponse.Response.Equals("Coupon History Saved"))
        //    {

        //    }
        //}
        #endregion
        public ActionResult OrderSuccessful()
        {
            LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            if (MdUser.Id == 0)
            {
                return RedirectToAction("Index", "WebHome");
            }
            return View(GetLastPlacedOrder());
        }
        public ActionResult DeleteAddress(int? id)
        {
            AddressModel objModel = new AddressModel();
            LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            objModel.UserId = MdUser.Id;
            objModel.Id = id;
            objModel.Operation = "delete";
            var _request = JsonConvert.SerializeObject(objModel);
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiSaveAddress, _request);
            return RedirectToAction("index");
        }
        public AddressModel BindCartItems()
        {
            AddressModel objModel = new AddressModel();
            ViewBag.ProductCatList = CommonFile.GetProductCategory(null);
            List<AddToCartModel> ListAddToCart = Services.GetMyCart(this.ControllerContext.HttpContext, _JwtTokenManager);
            ViewBag.AddToCart = ListAddToCart;
            objModel.CartItemCount = ListAddToCart.Count();
            ViewBag.TotalAmount = GetTotalAmount(ListAddToCart);
            return objModel;
        }
        public decimal GetTotalAmount(List<AddToCartModel> ListCart)
        {
            AddToCart objAddToCart = new AddToCart();
            return objAddToCart.GetTotalAmount(ListCart);
        }
        public SaleOrderModel GetLastPlacedOrder()
        {
            LoginModel MdUser = Services.GetLoginWebUser(this.ControllerContext.HttpContext, _JwtTokenManager);
            SaleOrderModel objmodel = new SaleOrderModel();
            objmodel.UserId = MdUser.Id;
            var _request = JsonConvert.SerializeObject(objmodel);
            ResponseModel ObjResponse = CommonFile.GetApiResponse(Constant.ApiGetOrders, _request);
            var Orders = JsonConvert.DeserializeObject<List<SaleOrderModel>>(ObjResponse.Response);
            objmodel.OrderNo = Orders.First().OrderNo;
            objmodel.SaleOrderId = Orders.First().Id;
            var _secondrequest = JsonConvert.SerializeObject(objmodel);
            ResponseModel ObjSecondResponse = CommonFile.GetApiResponse(Constant.ApiGetOrders, _secondrequest);
            ViewBag.OrderDetails = JsonConvert.DeserializeObject<List<SaleOrderModel>>(ObjSecondResponse.Response);
            return objmodel;
        }
    }
}