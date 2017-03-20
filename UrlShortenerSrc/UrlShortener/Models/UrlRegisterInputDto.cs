using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UrlShortener.Models
{
    /// <summary>
    /// Input object for /Register
    /// </summary>
    public class UrlRegisterInputDto
    {
        /// <summary>
        /// Real address to a HTTP resource
        /// </summary>
        [Required]
        public string Url { get; set; }
        /// <summary>
        /// Redirect type to use when redirecting
        /// Valid values: 301, 302
        /// </summary>
        public int RedirectType { get; set; } = 302;

        public bool IsValid()
        {
            return (RedirectType == 301 || RedirectType == 302) &&
                Uri.IsWellFormedUriString(Url, UriKind.Absolute);
        }
    }
}