using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using UrlShortener.Infrastructure;

namespace UrlShortener.DataAccess
{
    /// <summary>
    /// Service for URL-related actions
    /// </summary>
    public class UrlShortenerDictionaryService
    {
        private ConcurrentDictionary<string, Url> UrlStore { get; set; }
        private int idCount = 10000;  //To emulate a fuller database, for nicer short URLs
        private readonly char[] ValidShortUrlCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();


        public UrlShortenerDictionaryService()
        {
            UrlStore = new ConcurrentDictionary<string, Url>();
        }

        /// <summary>
        /// Register an URL address
        /// </summary>
        /// <param name="url">Address to redirect to</param>
        /// <param name="redirectType">HTTP status code for redirection</param>
        /// <param name="username">Username of the user doing the registration</param>
        /// <returns>Key for short URL, null if unsuccessful</returns>
        public async Task<string> StoreUrl(string url, int redirectType, string username)
        {
            if (redirectType != 301 && redirectType != 302)
            {
                throw new ArgumentOutOfRangeException("redirectType");
            }
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Argument invalid", "username");
            }
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new ArgumentException("Argument invalid", "url");
            }
            try
            {
                var id = Interlocked.Increment(ref idCount);
                var key = IdToShortUrl(id);
                var newUrl = new Url()
                {
                    Id = id,
                    Address = url,
                    RedirectType = redirectType,
                    Username = username
                };

                var success = UrlStore.TryAdd(key, newUrl);
                if (success)
                {
                    return key;
                }
            }
            catch (Exception ex)
            {
                Services.GetLogger(this).Error(ex);
            }
            return null;
        }

        /// <summary>
        /// Retrieves an URL model from UrlStore
        /// </summary>
        /// <param name="key">Key for short URL</param>
        /// <returns>URL model</returns>
        public async Task<IUrl> GetUrl(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Argument invalid", "key");
            }
            try
            {
                Url existingUrl;
                if (UrlStore.TryGetValue(key, out existingUrl))
                {
                    existingUrl.IncreaseUseCount();
                    return existingUrl;
                }
            }
            catch (Exception ex)
            {
                Services.GetLogger(this).Error(ex);
            }
            return null;
        }
        
        /// <summary>
        /// Retrieves all URL models belonging to a certain user
        /// </summary>
        /// <param name="username">AccountId/username filter</param>
        /// <returns>A collection of URL models</returns>
        public async Task<IEnumerable<IUrl>> GetAllUrlsForUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Argument invalid", "username");
            }
            try
            {
                return UrlStore.Values.Where(t => t.Username == username);
            }
            catch (Exception ex)
            {
                Services.GetLogger(this).Error(ex);
            }
            return null;
        }

        /// <summary>
        /// Converts an integer to a string representation of the number in base-36
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private string IdToShortUrl(int num)
        {
            var base36 = new StringBuilder();
            do
            {
                base36.Insert(0, ValidShortUrlCharacters[(int)(num % ValidShortUrlCharacters.Length)]);
                num = num / ValidShortUrlCharacters.Length;
            } while (num != 0);
            return base36.ToString();
        }

    }
}