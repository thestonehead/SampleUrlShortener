using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using UrlShortener.DataAccess;
using UrlShortener.Infrastructure;

namespace UrlShortener
{
    public class BasicAuthenticationFilter : IAuthenticationFilter
    {
        const string AuthenticationScheme = "Basic";
        public bool AllowMultiple { get { return false; } }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            //No authentication present
            if (context.Request.Headers?.Authorization?.Scheme != AuthenticationScheme)
            {
                return;
            }

            //Authentication header is present and Basic authentication is set, but there are no credentials
            if (String.IsNullOrEmpty(context.Request.Headers.Authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing authentication data.", context.Request);
                return;
            }
            string[] credentials;
            try
            {
                credentials = Encoding.UTF8.GetString(Convert.FromBase64String(context.Request.Headers.Authorization.Parameter)).Split(':');
                if (credentials.Length != 2)
                {
                    context.ErrorResult = new AuthenticationFailureResult("Invalid authentication data.", context.Request);
                    return;
                }
            }
            catch (Exception ex)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid authentication data.", context.Request);
                return;
            }

            if (!await Services.User.AuthenticateUser(credentials[0], credentials[1]))
            {
                context.ErrorResult = new AuthenticationFailureResult("Authentication denied.", context.Request);
                return;
            }


            context.Principal = new GenericPrincipal(new GenericIdentity(credentials[0]), null);
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return;
        }
    }

    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
        }

        public string ReasonPhrase { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            response.ReasonPhrase = ReasonPhrase;
            return response;
        }
    }
}