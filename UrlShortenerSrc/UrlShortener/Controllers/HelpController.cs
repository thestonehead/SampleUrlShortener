using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UrlShortener.Controllers
{
    [RoutePrefix("help")]
    public class HelpController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            return Redirect(Url.Content("Static/HelpPage.html"));
        }
    }
}
