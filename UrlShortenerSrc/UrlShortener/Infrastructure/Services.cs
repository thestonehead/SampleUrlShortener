using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UrlShortener.DataAccess;

namespace UrlShortener.Infrastructure
{
    /// <summary>
    /// Static class for accessing services
    /// </summary>
    public static class Services
    {
        /// <summary>
        /// URL-related service
        /// </summary>
        public static UrlShortenerDictionaryService Url { get; private set; }
        /// <summary>
        /// User-related service
        /// </summary>
        public static UserDictionaryService User { get; private set; }

        static Services()
        {
            Url = new UrlShortenerDictionaryService();
            User = new UserDictionaryService();
        }

        /// <summary>
        /// Get Logger for error logging
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static log4net.ILog GetLogger(object source)
        {
            return log4net.LogManager.GetLogger(source.GetType());
        }

    }
}