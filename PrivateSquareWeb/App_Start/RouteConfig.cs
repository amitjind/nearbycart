using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PrivateSquareWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "WebHome", action = "Index", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    name: "ForegetPassword",
            //    url: "checkpoint/{controller}/{token}",
            //    defaults: new { controller = "Login", action = "ResetPasword", token =UrlParameter.Optional}
            //);
        }
    }
}
