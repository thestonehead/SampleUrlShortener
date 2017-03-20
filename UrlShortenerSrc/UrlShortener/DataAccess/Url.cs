using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Web;

namespace UrlShortener.DataAccess
{
    public class Url : IUrl
    {
        private int userCount;
        public int Id { get; set; }

        public string Address { get; set; }

        public int UseCount { get { return userCount; } }

        public int RedirectType { get; set; }

        /// <summary>
        /// User who created this entry
        /// </summary>
        public string Username { get; set; }

        internal void IncreaseUseCount()
        {
            Interlocked.Increment(ref userCount);
        }
    }
}