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
    [RoutePrefix("account")]
    public class AccountController : ApiController
    {
        /// <summary>
        /// User account registration
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("")]
        [ValidateModelState]
        public async Task<AccountResponseDto> Post(AccountInputDto input)
        {
            //Check for validity
            if (!input.IsValid())
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    ReasonPhrase = "Input data is invalid."
                });
            }

            var result = await Services.User.RegisterUser(input.AccountId);
            return new AccountResponseDto(result);
        }
    }
}
