using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace UrlShortener
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            log4net.LogManager.GetLogger("test").Error("Test error");

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
