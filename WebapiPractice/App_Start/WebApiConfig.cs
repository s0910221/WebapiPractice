using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebapiPractice.Filters;

namespace WebapiPractice
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 設定和服務
            config.Filters.Add(new CustomExceptionAttribute());


            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
