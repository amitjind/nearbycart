
using PrivatesquaresWebApiNew.Models;
using PrivatesquaresWebApiNew.Persistance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivatesquaresWebApiNew.Persistance.Repositary
{
    public class UserRepositary : GenericRepository<EWT_PSQNEWEntities>, IUserRepositary
    {
        public ChangePassword_Result ChangePassword(LoginModel objModel)
        {
            return Context.ChangePassword(objModel.Id, objModel.Password, objModel.NewPassword).FirstOrDefault();

        }

        public ForgetPassword_Result ForgetPassword(LoginModel objModel)
        {
            return Context.ForgetPassword(objModel.EmailId, objModel.Password).FirstOrDefault();
        }



        public IEnumerable<GetAllInterest_Result> GetAllInterest()
        {
            return Context.GetAllInterest().ToList();
        }

        public IEnumerable<InterestCategories_Result> GetAllInterestCategory()
        {
            return Context.InterestCategories().ToList();
        }

        public IEnumerable<GetUserBusiness_Result> GetBusiness(BusinessModel objModel)
        {
            return Context.GetUserBusiness(objModel.UserId).ToList();
        }

        public IEnumerable<GetBusinessDetail_Result> GetBusinessDetail(BusinessModel objModel)
        {
            return Context.GetBusinessDetail(objModel.Id).ToList();
        }

        public IEnumerable<GetCity_Result> GetCity(DropDownModel objModel)
        {
            return Context.GetCity(objModel.StateId).ToList();
        }

        public IEnumerable<GetCountry_Result> GetCountry()
        {
            return Context.GetCountry().ToList();
        }

        public IEnumerable<GetNetwork_Result> GetNetwork(NetworkModel networkModel)
        {
            return Context.GetNetwork(networkModel.LogInUserId).ToList();
        }



        public IEnumerable<GetProduct_Result> GetProduct(ProductModel objModel)
        {
            return Context.GetProduct(objModel.UserId).ToList();
        }

        public IEnumerable<GetProductCategory_Result> GetProductCategory(DropDownModel objModel)
        {
            return Context.GetProductCategory(objModel.ParentCatId).ToList();
        }

        public IEnumerable<GetProductDetail_Result> GetProductDetail(ProductModel objModel)
        {
            return Context.GetProductDetail(objModel.Id, objModel.UserId).ToList();
        }

        public IEnumerable<GetProfession_Result> GetProfession()
        {
            return Context.GetProfession().ToList();
        }

        public IEnumerable<GetProfessionalKeyword_Result> GetProfessionalKeyword(DropDownModel objModel)
        {
            return Context.GetProfessionalKeyword().ToList();
        }

        public IEnumerable<GetState_Result> GetState(DropDownModel objModel)
        {
            return Context.GetState(objModel.CountryId).ToList();
        }

        public IEnumerable<GetAddress_Result> GetUserAddress(AddressModel objModel)
        {
            return Context.GetAddress(objModel.UserId, objModel.Id).ToList();
        }

        public IEnumerable<GetUserBusiness_Result> GetUserbusiness(BusinessModel objModel)
        {
            return Context.GetUserBusiness(objModel.UserId).ToList();
        }

        public IEnumerable<GetUserInterest_Result> GetUserInterest(UserInterestModel objModel)
        {
            return Context.GetUserInterest(objModel.UserId).ToList();
        }

        public IEnumerable<GetUserProfile_Result> GetUserProfile(UserProfileModel objModel)
        {
            return Context.GetUserProfile(objModel.UserId).ToList();
        }

        public IsBusinessExist_Result IsBusinessExist(BusinessModel objModel)
        {
            return Context.IsBusinessExist(objModel.BusinessName).FirstOrDefault();
        }

        public IsEmailExist_Result IsEmailExist(LoginModel objModel)
        {
            return Context.IsEmailExist(objModel.EmailId).FirstOrDefault();

        }

        public LoginAuthenticate_Result LoginAuthenticte(UserRegisterModel ObjModel)
        {
            var RESULT = Context.LoginAuthenticate(ObjModel.Mobile, ObjModel.Otp);
            return RESULT.FirstOrDefault();
        }

        public AuthenticateLogin_Result LoginUser(LoginModel objModel)
        {
            return Context.AuthenticateLogin(objModel.EmailId, objModel.Mobile, objModel.Password).FirstOrDefault();
        }
        public RegisterNewUser_Result RegisterNewUser(LoginModel objModel)
        {
            return Context.RegisterNewUser(objModel.Name, objModel.EmailId, objModel.Mobile, objModel.Password, objModel.RegisterType).FirstOrDefault();
        }

        public RegisterUser_Result RegisterUser(UserRegisterModel ObjModel)
        {
            return Context.RegisterUser(ObjModel.Mobile, ObjModel.Operation).FirstOrDefault();
        }

        public SaveAddress_Result SaveAddress(AddressModel objModel)
        {
            return Context.SaveAddress(objModel.Id, objModel.UserId, objModel.Name, objModel.Mobile, objModel.Pincode, objModel.Locality, objModel.Address, objModel.CityId, objModel.StateId, objModel.Landmark, objModel.AlternatePhone, objModel.Operation).FirstOrDefault();
        }



        public SaveBusiness_Result SaveBusiness(BusinessModel ObjModel)
        {
            return Context.SaveBusiness(ObjModel.Id, ObjModel.BusinessName, ObjModel.Location, ObjModel.BusinessLogo, ObjModel.ProfessionalCatId,
                ObjModel.ProfessionalKeyword, ObjModel.CityId, ObjModel.PinCode, ObjModel.UserId, ObjModel.Email, ObjModel.Description, ObjModel.Phone, ObjModel.CountryId, ObjModel.Website, ObjModel.Operation).FirstOrDefault();

        }

        public SaveContactUs_Result SaveContactUs(ContactUsModel objModel)
        {
            return Context.SaveContactUs(objModel.FullName, objModel.Mobile, objModel.Email, objModel.Message).FirstOrDefault();
        }

        public SaveNetwork_Result SaveNetwork(NetworkModel objModel)
        {
            return Context.SaveNetwork(objModel.LogInUserId, objModel.UserId, objModel.UserProfileId, objModel.Operation).FirstOrDefault();


        }



        public SaveProduct_Result SaveProduct(ProductModel objModel)
        {
            return Context.SaveProduct(objModel.Id, objModel.ProductName, objModel.ProductCatId, objModel.ProductImage,
                objModel.SellingPrice, objModel.DiscountPrice, objModel.BusinessId, objModel.UserId, objModel.Description,
                objModel.Operation, objModel.XmlProductImage).FirstOrDefault();

        }

        public SaveProfile_Result SaveProfile(UserProfileModel ObjModel)
        {
            return Context.SaveProfile(ObjModel.UserId, ObjModel.FirstName, ObjModel.LastName, ObjModel.ProfileImage, ObjModel.Description, ObjModel.EmailId, ObjModel.ProfessionalCatId, ObjModel.Title, ObjModel.ProfessionalKeyword, ObjModel.CityId, ObjModel.Password, ObjModel.GenderId, ObjModel.DOB, ObjModel.Location, ObjModel.Phone, ObjModel.Pincode, ObjModel.CountryId, ObjModel.OfficeAddress, ObjModel.OtherAddress, ObjModel.InterestCatId, ObjModel.XmlData, ObjModel.XmlDataAddress).FirstOrDefault();
        }

        public SaveUserForgetPasswordLink_Result SaveUserForgetPasswordLink(LoginModel objModel)
        {
            return Context.SaveUserForgetPasswordLink(objModel.EmailId, objModel.Operation).FirstOrDefault();
        }

        public SaveUserInterest_Result SaveUserInterest(UserInterestModel objModel)
        {
            var Result = Context.SaveUserInterest(objModel.XmlData, objModel.Operation);
            return Result.FirstOrDefault();
        }

        public SaveUserInterest_Result UpdateUserInterest(UserInterestModel objModel)
        {
            var Result = Context.SaveUserInterest(objModel.XmlData, objModel.Operation);

            return Result.FirstOrDefault();
        }

        IEnumerable<GetUsersProfileList_Result> IUserRepositary.GetUserProfileList(UsersProfileModel objModel)
        {
            return Context.GetUsersProfileList(objModel.UserId).ToList();
        }
        public IEnumerable<GetOrders_Result> GetOrders(SaleOrderModel objModel)

        {

            return Context.GetOrders(objModel.UserId, objModel.SaleOrderId, objModel.ProductId).ToList();

        }

        public IEnumerable<GetAddToCart_Result> GetAddToCart(AddToCartModel objModel)

        {
            throw new NotImplementedException();
            // return Context.GetAddToCart(objModel.UserId).ToList();

        }



        public SaveOrders_Result SaveOrders(SaleOrderModel objModel)

        {

            return Context.SaveOrders(objModel.UserId, objModel.TotalDiscount, objModel.TotalAmount, objModel.PaymentMode, objModel.XmlSaleOrderDetail, objModel.Operation, objModel.SaleOrderId).FirstOrDefault();

        }

        public SaveAddToCart_Result SaveAddToCart(AddToCartModel objModel)
        {
            return Context.SaveAddToCart(objModel.XmlCartDetails, objModel.Operation).FirstOrDefault();
        }

        public IEnumerable<GetPopularProductId_Result> GetPopularProductId(ProductModel objModel)
        {
            return Context.GetPopularProductId(objModel.ProductCatId).ToList();
        }

        public IEnumerable<GetWishlist_Result> GetWishlist(AddToCartModel objModel)
        {
            return Context.GetWishlist(objModel.UserId).ToList();
        }

        public SaveWishlist_Result SaveWishlist(AddToCartModel objModel)
        {
            return Context.SaveWishlist(objModel.UserId,objModel.ProductId,objModel.Operation).FirstOrDefault();
        }

        public IEnumerable<GetSortedProducts_Result> GetSortedProducts(ProductModel objModel)
        {
            return Context.GetSortedProducts(objModel.Sortby,objModel.PageIndex,objModel.ProductCatId).ToList();
        }

        public SaveReview_Result SaveReview(ContactUsModel objModel)
        {
            return Context.SaveReview(objModel.ProductId, objModel.UserId, objModel.TotalRating, objModel.GivenRating, objModel.Review, objModel.Operation).FirstOrDefault();
        }

        public IEnumerable<GetReview_Result> GetReview(ContactUsModel objModel)
        {
            return Context.GetReview(objModel.ProductId).ToList();
        }

        public IEnumerable<GetCoupon_Result> GetCoupon(CouponModel objModel)
        {
            return Context.GetCoupon(objModel.CouponCode,objModel.UserId).ToList();
        }

        public SaveCouponHistory_Result SaveCouponHistory(CouponModel objModel)
        {
            return Context.SaveCouponHistory(objModel.CouponId, objModel.UserId, objModel.OrderId, objModel.Discount, objModel.Operation).FirstOrDefault();
        }

        public IsParentCategory_Result IsParentCategory(ProductModel objModel)
        {
            return Context.IsParentCategory(objModel.ProductCatId).FirstOrDefault();
        }

        public IEnumerable<GetChildCategory_Result> GetChildCategory(ProductModel objModel)
        {
            return Context.GetChildCategory(objModel.ProductCatId).ToList();
        }
    }

    public interface IUserRepositary : IGenericRepository<EWT_PSQNEWEntities>
    {
        RegisterUser_Result RegisterUser(UserRegisterModel ObjModel);
        LoginAuthenticate_Result LoginAuthenticte(UserRegisterModel ObjModel);
        SaveProfile_Result SaveProfile(UserProfileModel ObjModel);
        SaveBusiness_Result SaveBusiness(BusinessModel ObjModel);
        SaveUserInterest_Result SaveUserInterest(UserInterestModel objModel);
        SaveProduct_Result SaveProduct(ProductModel objModel);
        IEnumerable<GetUsersProfileList_Result> GetUserProfileList(UsersProfileModel objModel);
        IEnumerable<GetAllInterest_Result> GetAllInterest();
        IEnumerable<InterestCategories_Result> GetAllInterestCategory();
        SaveNetwork_Result SaveNetwork(NetworkModel objModel);
        IEnumerable<GetNetwork_Result> GetNetwork(NetworkModel networkModel);
        IEnumerable<GetUserBusiness_Result> GetUserbusiness(BusinessModel objModel);
        IEnumerable<GetUserProfile_Result> GetUserProfile(UserProfileModel objModel);
        IEnumerable<GetUserBusiness_Result> GetBusiness(BusinessModel objModel);

        IEnumerable<GetProduct_Result> GetProduct(ProductModel objModel);

        IEnumerable<GetUserInterest_Result> GetUserInterest(UserInterestModel objModel);

        SaveUserInterest_Result UpdateUserInterest(UserInterestModel objModel);
        IEnumerable<GetProfession_Result> GetProfession();
        IEnumerable<GetCountry_Result> GetCountry();
        IEnumerable<GetState_Result> GetState(DropDownModel objModel);
        IEnumerable<GetCity_Result> GetCity(DropDownModel objModel);
        IEnumerable<GetProductCategory_Result> GetProductCategory(DropDownModel objModel);
        IEnumerable<GetProductDetail_Result> GetProductDetail(ProductModel objModel);
        IEnumerable<GetBusinessDetail_Result> GetBusinessDetail(BusinessModel objModel);
        AuthenticateLogin_Result LoginUser(LoginModel objModel);
        RegisterNewUser_Result RegisterNewUser(LoginModel objModel);
        ForgetPassword_Result ForgetPassword(LoginModel objModel);
        ChangePassword_Result ChangePassword(LoginModel objModel);
        SaveContactUs_Result SaveContactUs(ContactUsModel objModel);

        IEnumerable<GetProfessionalKeyword_Result> GetProfessionalKeyword(DropDownModel objModel);
        IsEmailExist_Result IsEmailExist(LoginModel objModel);
        IsBusinessExist_Result IsBusinessExist(BusinessModel objModel);
        SaveUserForgetPasswordLink_Result SaveUserForgetPasswordLink(LoginModel objModel);
        IEnumerable<GetAddress_Result> GetUserAddress(AddressModel objModel);
        SaveAddress_Result SaveAddress(AddressModel objModel);
        SaveOrders_Result SaveOrders(SaleOrderModel objModel);

        SaveAddToCart_Result SaveAddToCart(AddToCartModel objModel);

        IEnumerable<GetAddToCart_Result> GetAddToCart(AddToCartModel objModel);

        IEnumerable<GetOrders_Result> GetOrders(SaleOrderModel objModel);

        IEnumerable<GetPopularProductId_Result> GetPopularProductId(ProductModel objModel);
        IEnumerable<GetWishlist_Result> GetWishlist(AddToCartModel objModel);
        SaveWishlist_Result SaveWishlist(AddToCartModel objModel);
        IEnumerable<GetSortedProducts_Result> GetSortedProducts(ProductModel objModel);
        SaveReview_Result SaveReview(ContactUsModel objModel);
        IEnumerable<GetReview_Result> GetReview(ContactUsModel objModel);
        IEnumerable<GetCoupon_Result> GetCoupon(CouponModel objModel);
        SaveCouponHistory_Result SaveCouponHistory(CouponModel objModel);
        IsParentCategory_Result IsParentCategory(ProductModel objModel);
        IEnumerable<GetChildCategory_Result> GetChildCategory(ProductModel objModel);
    }

}