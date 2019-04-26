using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateSquareWeb
{
	public class Constant
	{
		public const string DomainUrl = " http://nearbycart.com/";//http://localhost:53693/        http://192.168.1.150:2552/
		public const string ApiRegister = "User/Registeration";// For Mobile Login
		public const string ApiLogin = "User/AuthenticateUser";// For Mobile With Otp
		public const string ApiLoginUser = "User/LoginUser";
		public const string ApiRegisterUser = "User/RegisterUser";
		public const string ApiGetAllInterestCategory = "User/GetAllInterestCategory";
		public const string ApiGetAllInterest = "User/GetAllInterest";
		public const string ApiGetUsersProfile = "User/GetUsersProfile";
		public const string ApiSaveUserInterest = "User/SaveUserInterest";
		public const string ApiGetUserProfile = "User/GetUserProfile";
		public const string ApiSaveProfile = "User/SaveProfile";
		public const string ApiSaveBusiness = "User/SaveBusiness";
		public const string ApiGetUserBusiness = "User/GetUserBusiness";
		public const string ApiGetBusiness = "User/GetBusiness";
		public const string ApiGetBusinessDetail = "User/GetBusinessDetail";

		public const string ApiSaveProduct = "User/SaveProduct";
		public const string ApiSaveNetwork = "User/SaveNetwork";
		public const string ApiGetUserNetwork = "User/GetNetwork";
		public const string ApiGetProfile = "User/GetProfile";
		public const string ApiGetProfession = "User/GetProfession";
		public const string ApiGetProduct = "User/GetProduct";
		public const string ApiGetProductDetail = "User/GetProductDetail";
		public const string ApiGetCountry = "User/GetCountry";
		public const string ApiGetState = "User/GetState";
		public const string ApiGetCity = "User/GetCity";
		public const string ApiGetProductCategory = "User/GetProductCategory";
		public const string ApiForgetPassword = "User/ForgetPassword";
		public const string ApiGetUserInterest = "User/GetUserInterest";
		public const string ApiSaveContactUs = "User/SaveContactUs";
		public const string ApiChangePassword = "User/ChangePassword";
		public const string ApiGetProfessionalKeyword = "User/GetProfessionalKeyword";
		public const string ApiIsEmailExist = "User/IsEmailExist";
		public const string ApiIsBusinessExist = "User/IsBusinessExist";
		public const string ApiSaveUserForgetPasswordLink = "User/SaveUserForgetPasswordLink";
		public const string ApiGetUserAddress = "User/GetUserAddress";
		public const string ApiSaveAddress = "User/SaveAddress";
		public const string ApiSaveOrders = "User/SaveOrders";
		public const string ApiSaveAddToCart = "User/SaveAddToCart";
		public const string ApiGetAddToCart = "User/GetAddToCart";
		public const string ApiGetOrders = "User/GetOrders";
		public const string ApiGetPopularProductId = "User/GetPopularProductId";
		public const string ApiSaveWishlist = "User/SaveWishlist";
		public const string ApiGetWishlist = "User/GetWishlist";
		public const string ApiGetSortedProducts = "User/GetSortedProducts";
		public const string ApiSaveReview = "User/SaveReview";
		public const string ApiGetReview = "User/GetReview";
		public const string ApiGetCoupon = "User/GetCoupon";
		public const string ApiSaveCouponHistory = "User/SaveCouponHistory";
		public const string ApiIsParentCategory = "User/IsParentCategory";
		public const string ApiGetChildCategory = "User/GetChildCategory";
		public const int NumberOfProducts = 12;
		public const int NumberOfProductsInFrontPage = 4;
		public static int[] ParentCategories =new int[] { 36, 37, 38, 39, 41, 42 };
		public const int MinimumAmountForFreeDelivery = 500;
		public const int ShippingCharges = 50;
		public static string EncodeNumber(int id)
		{
			byte[] encoded = System.Text.Encoding.UTF8.GetBytes(id.ToString());
			return Convert.ToBase64String(encoded);
		
		}
	}
}