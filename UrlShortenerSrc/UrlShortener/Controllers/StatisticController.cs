using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using UrlShortener.DataAccess;
using UrlShortener.Infrastructure;

namespace UrlShortener.Controllers
{
    [RoutePrefix("statistic")]
    public class StatisticController : ApiController
    {
        /// <summary>
        /// Gets statistics of URL usage for a specific account
        /// </summary>
        /// <param name="accountId">Account identifier/Username</param>
        /// <returns>Key-value list with URLs and their use count</returns>
        [Route("{accountId}")]
        [Authorize]
        public async Task<Dictionary<string, int>> Get(string accountId)
        {
            var statistics = await Services.Url.GetAllUrlsForUser(accountId);
            if (!statistics.Any())
            {
                var user = await Services.User.GetUser(accountId);
                if (user == null)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        ReasonPhrase = "No such account"
                    });
                }
            }
            return statistics.GroupBy(t => t.Address).ToDictionary(k => k.Key, v => v.Sum(t => t.UseCount));
        }
    }
}
