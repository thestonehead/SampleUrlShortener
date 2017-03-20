using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UrlShortener.DataAccess;

namespace UrlShortener.Models
{
    /// <summary>
    /// Response object for /Account
    /// </summary>
    public class AccountResponseDto
    {
        /// <summary>
        /// True if a new account has been created
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Description of the result/exception
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Password for the newly created user
        /// Null if user hasn't been created
        /// </summary>
        public string Password { get; set; }

        public AccountResponseDto() { }

        public AccountResponseDto(RegisterUserResponse result) : this()
        {
            switch (result.Result)
            {
                case RegisterUserResponse.RegistrateUserResult.UserCreated:
                    Success = true;
                    Description = "Your account has been successfully opened.";
                    Password = result.Password;
                    break;
                case RegisterUserResponse.RegistrateUserResult.InvalidUsername:
                    Success = false;
                    Description = "Failure: username must contain at least one character.";
                    break;
                case RegisterUserResponse.RegistrateUserResult.UsernameAlreadyExists:
                    Success = false;
                    Description = "Failure: username already exists.";
                    break;
                case RegisterUserResponse.RegistrateUserResult.Unknown:
                default:
                    Success = false;
                    Description = "Failure: unknown exception occurred. Try again later or report the issue.";
                    break;
            }
        }
    }
}