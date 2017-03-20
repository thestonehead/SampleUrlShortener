using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace UrlShortener
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Filters.Add(new BasicAuthenticationFilter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            //Route for all shortUrls
            config.Routes.MapHttpRoute(
                name: "GetUrl",
                routeTemplate: "{shortUrl}",
                defaults: new
                {
                    controller = "Url",
                    action = "Get"
                });
        }
    }
}
