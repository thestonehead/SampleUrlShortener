using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using UrlShortener.DataAccess;
using UrlShortener.Infrastructure;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    public class UrlController : ApiController
    {
        /// <summary>
        /// Register an URL address
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("register")]
        [Authorize]
        [ValidateModelState]
        public async Task<UrlRegisterResponseDto> Post(UrlRegisterInputDto input)
        {
            //Check for validity
            if (!input.IsValid())
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    ReasonPhrase = "Input data is invalid."
                });
            }

            if (input.Url.Contains(Request.RequestUri.Authority))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    ReasonPhrase = "Circular redirection not supported."
                });
            }

            var resultId = await Services.Url.StoreUrl(input.Url, input.RedirectType, RequestContext.Principal.Identity.Name);

            if (resultId == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }

            return new UrlRegisterResponseDto(Url.Content(resultId.ToString()));
        }

        /// <summary>
        /// Redirect to real address
        /// </summary>
        /// <param name="shortUrl"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Get(string shortUrl)
        {
            var url = await Services.Url.GetUrl(shortUrl);
            if (url == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            HttpStatusCode statusCode = url.RedirectType == 301 ? HttpStatusCode.MovedPermanently : HttpStatusCode.Found;

            var response = Request.CreateResponse(statusCode);
            response.Headers.Location = new Uri(url.Address);
            return ResponseMessage(response);
        }
    }
}
