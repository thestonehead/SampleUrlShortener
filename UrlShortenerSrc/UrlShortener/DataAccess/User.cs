using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UrlShortener.DataAccess
{
    public class User
    {
        //public int Id { get; set; }
        public string Username { get; set; }
        public string PassHash { get; set; }
    }
}