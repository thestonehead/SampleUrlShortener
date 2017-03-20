using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UrlShortener.Models
{
    /// <summary>
    /// Response object for /Register
    /// </summary>
    public class UrlRegisterResponseDto
    {
        /// <summary>
        /// Short URL generated
        /// </summary>
        public string ShortUrl { get; set; }

        public UrlRegisterResponseDto() { }
        public UrlRegisterResponseDto(string shortUrl) : this()
        {
            ShortUrl = shortUrl;
        }
    }
}