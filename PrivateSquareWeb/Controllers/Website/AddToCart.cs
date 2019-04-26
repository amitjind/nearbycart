using Newtonsoft.Json;
using PrivateSquareWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateSquareWeb.Controllers.Website
{
    public class AddToCart : Controller
    {
        JwtTokenManager _JwtTokenManager = new JwtTokenManager();
        public JsonResult AddToCartFun(AddToCartModel objmodel, HttpContextBase httpContext)
        {
            List<AddToCartModel> ListAddtoCart = new List<AddToCartModel>();
            List<AddToCartModel> CookiesListAddtoCart = null;

            if (Services.GetCookie(httpContext, "addtocart") != null)
            {
                CookiesListAddtoCart = Services.GetMyCart(httpContext, _JwtTokenManager);
                //  ListAddtoCart.Add(objmodel);
                List<AddToCartModel> ListuniqueValues = uniqueValues(CookiesListAddtoCart, objmodel,false);
                // CookiesListAddtoCart.AddRange(ListAddtoCart);

                // var jsonList = JsonConvert.SerializeObject(CookiesListAddtoCart);
                var jsonList = JsonConvert.SerializeObject(ListuniqueValues);
                Services.SetCookie(httpContext, "addtocart", _JwtTokenManager.GenerateToken(jsonList));
                return Json(jsonList);
            }
            else
            {
                ListAddtoCart.Add(objmodel);
                var jsonList = JsonConvert.SerializeObject(ListAddtoCart);
                Services.SetCookie(httpContext, "addtocart", _JwtTokenManager.GenerateToken(jsonList));
                return Json(jsonList);
            }
        }
        public JsonResult RemoveQtyToCartFun(AddToCartModel objmodel, HttpContextBase httpContext)
        {
            List<AddToCartModel> ListAddtoCart = new List<AddToCartModel>();
            List<AddToCartModel> CookiesListAddtoCart = null;

            if (Services.GetCookie(httpContext, "addtocart") != null)
            {
                CookiesListAddtoCart = Services.GetMyCart(httpContext, _JwtTokenManager);
                //  ListAddtoCart.Add(objmodel);
                List<AddToCartModel> ListuniqueValues = uniqueValues(CookiesListAddtoCart, objmodel,true);
                // CookiesListAddtoCart.AddRange(ListAddtoCart);

                // var jsonList = JsonConvert.SerializeObject(CookiesListAddtoCart);
                var jsonList = JsonConvert.SerializeObject(ListuniqueValues);
                Services.SetCookie(httpContext, "addtocart", _JwtTokenManager.GenerateToken(jsonList));
                return Json(jsonList);
            }
            else
            {
                ListAddtoCart.Add(objmodel);
                var jsonList = JsonConvert.SerializeObject(ListAddtoCart);
                Services.SetCookie(httpContext, "addtocart", _JwtTokenManager.GenerateToken(jsonList));
                return Json(jsonList);
            }
        }
        private List<AddToCartModel> uniqueValues(List<AddToCartModel> ListCart, AddToCartModel objmodel,bool IsRemoveQty)
        {
            List<AddToCartModel> oldList = ListCart;
            List<AddToCartModel> Uniquelist = new List<AddToCartModel>();
            bool IsExist = false;
            if (ListCart != null && ListCart.Count > 0)
            {
                for (int i = 0; i < ListCart.Count; i++)
                {
                    AddToCartModel clsAddTocart = new AddToCartModel();
                    clsAddTocart.ProductId = ListCart[i].ProductId;
                    clsAddTocart.ProductName = ListCart[i].ProductName;
                    clsAddTocart.ImageName = ListCart[i].ImageName;
                    if (ListCart[i].ProductId == objmodel.ProductId)
                    {
                        if (IsRemoveQty == true)
                        {
                            clsAddTocart.Qty = ListCart[i].Qty - objmodel.Qty;
                        }
                        else
                        { 
                        clsAddTocart.Qty = ListCart[i].Qty + objmodel.Qty;
                        }
                        clsAddTocart.Price = ListCart[i].Price;
                        clsAddTocart.Amount = clsAddTocart.Qty * clsAddTocart.Price;
                        oldList[i] = clsAddTocart;
                        // Uniquelist.Add(clsAddTocart);
                        IsExist = true;
                        break;
                    }
                }
                if (IsExist == false)
                {
                    AddToCartModel clsAddTocart = new AddToCartModel();
                    clsAddTocart.ProductId = objmodel.ProductId;
                    clsAddTocart.ProductName = objmodel.ProductName;
                    clsAddTocart.ImageName = objmodel.ImageName;
                    clsAddTocart.Qty = objmodel.Qty;
                    clsAddTocart.Price = objmodel.Price;
                    clsAddTocart.Amount = clsAddTocart.Qty * clsAddTocart.Price;
                    //Uniquelist.Add(clsAddTocart);
                    oldList.Add(clsAddTocart);
                }
            }
            if (ListCart.Count == 0)
            {
                AddToCartModel clsAddTocart = new AddToCartModel();
                clsAddTocart.ProductId = objmodel.ProductId;
                clsAddTocart.ProductName = objmodel.ProductName;
                clsAddTocart.ImageName = objmodel.ImageName;
                clsAddTocart.Qty = objmodel.Qty;
                clsAddTocart.Price = objmodel.Price;
                clsAddTocart.Amount = clsAddTocart.Qty * clsAddTocart.Price;
                //Uniquelist.Add(clsAddTocart);
                oldList.Add(clsAddTocart);
            }
            //else
            //{
            //    clsAddTocart.Qty = objmodel.Qty;
            //    clsAddTocart.Price = objmodel.Price;
            //    clsAddTocart.Amount = clsAddTocart.Qty * clsAddTocart.Price;
            //    //Uniquelist.Add(clsAddTocart);
            //    oldList.Add(clsAddTocart);
            //    break;
            //}



            return oldList;
        }
        public JsonResult RemoveCart(int index, HttpContextBase httpContext)
        {
            List<AddToCartModel> CookiesListAddtoCart = null;
            CookiesListAddtoCart = Services.GetMyCart(httpContext, _JwtTokenManager);
            List<AddToCartModel> ListuniqueValues = ListRemoveValues(CookiesListAddtoCart, index);
            var jsonList = JsonConvert.SerializeObject(ListuniqueValues);
            Services.SetCookie(httpContext, "addtocart", _JwtTokenManager.GenerateToken(jsonList));
            return Json(jsonList);
        }
        private List<AddToCartModel> ListRemoveValues(List<AddToCartModel> ListCard, int index)
        {
            List<AddToCartModel> OldList = ListCard;
            OldList.RemoveAt(index);
            return OldList;
        }
        public decimal GetTotalAmount(List<AddToCartModel> ListCart)
        {
            decimal TotalAmount = 0;
            if (ListCart != null && ListCart.Count > 0)
            {
                for (int i = 0; i < ListCart.Count; i++)
                {
                    TotalAmount += ListCart[i].Amount;
                }
            }
            return TotalAmount;
        }
        public int GetItemCount(HttpContextBase httpContext)
        {
            List<AddToCartModel> CookiesListAddtoCart = null;
            CookiesListAddtoCart = Services.GetMyCart(httpContext, _JwtTokenManager);
            int ItemCount = CookiesListAddtoCart.Count();
            return ItemCount;
        }
        public decimal GetTotalAmountCheckOut(HttpContextBase httpContext)
        {
            decimal TotalAmount = 0;
            List<AddToCartModel> ListCart = null;
            ListCart = Services.GetMyCart(httpContext, _JwtTokenManager);
            if (ListCart != null && ListCart.Count > 0)
            {
                for (int i = 0; i < ListCart.Count; i++)
                {
                    TotalAmount += ListCart[i].Amount;
                }
            }
            return TotalAmount;
        }
    }
}