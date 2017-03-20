using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UrlShortener.Infrastructure;

namespace UrlShortener.DataAccess
{
    public class UserDictionaryService
    {
        private HashAlgorithm PasswordHashAlgorithm { get; set; }
        private ConcurrentDictionary<string, User> UserStore { get; set; }
        private readonly char[] ValidPasswordCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        private const int PasswordSize = 8;
        
        public UserDictionaryService()
        {
            PasswordHashAlgorithm = SHA512.Create();
            UserStore = new ConcurrentDictionary<string, User>();
        }

        /// <summary>
        /// Checks if username-password combination is valid
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> AuthenticateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            User user;
            if (UserStore.TryGetValue(username, out user))
            {
                if (user.PassHash == GeneratePassHash(password))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds a new user to the storage and returns their password
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<RegisterUserResponse> RegisterUser(string username)
        {
            //Username validation
            if (String.IsNullOrEmpty(username))
            {
                return new RegisterUserResponse()
                {
                    Result = RegisterUserResponse.RegistrateUserResult.InvalidUsername
                };
            }

            var password = GenerateRandomPassword();
            var passHash = GeneratePassHash(password);
            try
            {
                var newUser = new User()
                {
                    Username = username,
                    PassHash = passHash
                };

                //TryAdd fails if key already exists in the database
                if (UserStore.TryAdd(username, newUser))
                {
                    return new RegisterUserResponse()
                    {
                        Password = password,
                        Result = RegisterUserResponse.RegistrateUserResult.UserCreated
                    };
                }
                else
                {
                    return new RegisterUserResponse()
                    {
                        Result = RegisterUserResponse.RegistrateUserResult.UsernameAlreadyExists
                    };
                }
            }
            catch (Exception ex)
            {
                Services.GetLogger(this).Error(ex);
            }
            return new RegisterUserResponse()
            {
                Result = RegisterUserResponse.RegistrateUserResult.Unknown
            };
        }

        /// <summary>
        /// Fetches a user from the storage
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<User> GetUser(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                return null;
            }
            User user;
            if (UserStore.TryGetValue(username, out user))
            {
                return user;
            }
            return null;
        }

        /// <summary>
        /// Generates random password by using cryptographically safe RNG
        /// </summary>
        /// <returns></returns>
        private string GenerateRandomPassword()
        {
            using (var random = new RNGCryptoServiceProvider())
            {
                byte[] randomByteArray = new byte[PasswordSize];
                random.GetNonZeroBytes(randomByteArray);
                return new string(randomByteArray.Select(t => ValidPasswordCharacters[(int)t % ValidPasswordCharacters.Length]).ToArray());
            }
        }

        /// <summary>
        /// Generates a password hash using a hash algorithm
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string GeneratePassHash(string password)
        {
            return Encoding.UTF8.GetString(PasswordHashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }

    /// <summary>
    /// Registration response object
    /// </summary>
    public class RegisterUserResponse
    {
        /// <summary>
        /// Password if user has been successfully created
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// User registration status
        /// </summary>
        public RegistrateUserResult Result { get; set; }

        public enum RegistrateUserResult
        {
            UserCreated,
            InvalidUsername,
            UsernameAlreadyExists,
            Unknown
        }
    }
}
