using Newtonsoft.Json;
using PrivateSquareWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace PrivateSquareWeb
{
    public class Services
    {
        public static string ApiUrl => Properties.Settings.Default.ApiUrl;
        #region Api Response

        public static HttpWebResponse GetApiResponse(string url, string metthod, string postData)
        {
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();

                byte[] data = encoding.GetBytes(string.Concat("{Data:\"", postData, "\"}"));

                // var dataRequest = new RequestModel
                //{
                //    Data = postData
                //};
                //byte[] data = encoding.GetBytes(dataRequest.Data);

                //  string encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Properties.Settings.Default.DegaAPIUser + ":" + Properties.Settings.Default.DegaAPIPassword));

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(string.Format(ApiUrl, url)));
                request.Method = metthod;


                request.ContentType = "application/json";
                request.ContentLength = data.Length;


                //   request.Headers.Add("Authorization", "Basic " + encoded);


                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    return ((HttpWebResponse)request.GetResponse());
                }
            }
            catch (WebException ex)
            {
                return ((HttpWebResponse)ex.Response);
            }
        }

        public static string GetApiResponseJson(string url, string metthod, string postData)

        {

            using (var http = new HttpClient())
            {
                // Define authorization headers here, if any
                // http.DefaultRequestHeaders.Add("Authorization", authorizationHeaderValue);
                var data = new RequestModel
                {
                    Data = postData
                };
                var content = new StringContent(JsonConvert.SerializeObject(data));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var request = http.PostAsync(string.Format(ApiUrl, url), content);

                var response = request.Result.Content.ReadAsStringAsync().Result;
                if (request.Result.IsSuccessStatusCode)
                    return response;
                else
                {
                    return "No Data Found";
                }
                //  return JsonConvert.DeserializeObject<ResponseModelType>(response);
            }

        }
        public static void SetCookie(HttpContextBase httpContext, string name, string value)
        {
            HttpCookie cookie = new HttpCookie(name, value);
            httpContext.Response.Cookies.Add(cookie);

        }
        public static void RemoveCookie(HttpContextBase httpContext, string name)
        {
            //System.Web.HttpContext.Response.Cookies.Remove(cookieName); // for example .ASPXAUTH
            HttpCookie cookie = new HttpCookie(name);
            cookie.Expires = DateTime.Now.AddDays(-1);
            httpContext.Response.Cookies.Add(cookie);

        }
        public static HttpCookie GetCookie(HttpContextBase httpContext, string name)
        {
            if (httpContext.Request.Cookies.AllKeys.Contains(name))
            {
                // return httpContext.Request.Cookies.Get(name);
                return httpContext.Request.Cookies[name];

            }
            else
                return null;


        }
        public static LoginModel GetLoginUser(HttpContextBase httpContext, JwtTokenManager jwtTokenManager)
        {
            LoginModel login =new LoginModel();
            if (Services.GetCookie(httpContext, "usr") == null)
                return login;
            string cookiesValue = Services.GetCookie(httpContext, "usr").Value;
            dynamic _data = jwtTokenManager.DecodeToken(cookiesValue);
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(_data);
            if (json.ContainsKey("unique_name"))
            {
                login = JsonConvert.DeserializeObject<LoginModel>(json["unique_name"].ToString());

            }
            return login;
        }
        public static LoginModel GetLoginWebUser(HttpContextBase httpContext, JwtTokenManager jwtTokenManager)
        {
            LoginModel login = new LoginModel();
            if (Services.GetCookie(httpContext, "webusr") == null)
                return login;
            string cookiesValue = Services.GetCookie(httpContext, "webusr").Value;
            if(string.IsNullOrWhiteSpace(cookiesValue))
                return login;
            dynamic _data = jwtTokenManager.DecodeToken(cookiesValue);
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(_data);
            if (json.ContainsKey("unique_name"))
            {
                login = JsonConvert.DeserializeObject<LoginModel>(json["unique_name"].ToString());

            }
            return login;
        }
        public static List<AddToCartModel> GetMyCart(HttpContextBase httpContext, JwtTokenManager jwtTokenManager)
        {
            List<AddToCartModel> myCart = new List<AddToCartModel>();
            if (Services.GetCookie(httpContext, "addtocart") == null)
                return myCart;
            string cookiesValue = Services.GetCookie(httpContext, "addtocart").Value;
            dynamic _data = jwtTokenManager.DecodeToken(cookiesValue);
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(_data);
            if (json.ContainsKey("unique_name"))
            {
                myCart = JsonConvert.DeserializeObject<List<AddToCartModel>>(json["unique_name"].ToString());

            }
            return myCart;
        }
        //public static UserModel GetLoginUser(HttpContextBase httpContext, JwtTokenManager jwtTokenManager)
        //{
        //    UserModel login = null;
        //    string cookiesValue = Services.GetCookie(httpContext, "usr").Value;
        //    dynamic _data = jwtTokenManager.DecodeToken(cookiesValue);
        //    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(_data);
        //    if (json.ContainsKey("unique_name"))
        //    {
        //        login = JsonConvert.DeserializeObject<UserModel>(json["unique_name"].ToString());

        //    }
        //    return login;
        //}

        #endregion

    }
}