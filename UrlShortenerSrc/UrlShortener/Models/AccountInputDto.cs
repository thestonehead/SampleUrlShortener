using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UrlShortener.Models
{
    /// <summary>
    /// Input data for /Account
    /// </summary>
    public class AccountInputDto
    {
        /// <summary>
        /// Account identification
        /// It has to be at least one character long
        /// </summary>
        [Required]
        public string AccountId { get; set; }

        /// <summary>
        /// Checks data for validity
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !String.IsNullOrEmpty(AccountId);
        }
    }
}