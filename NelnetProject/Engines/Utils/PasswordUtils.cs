using System;
using System.Security.Cryptography;
using System.Text;

namespace Engines.Utils
{
    /// <summary>
    /// Utility class for hashing passwords
    /// </summary>
    public static class PasswordUtils
    {
        private static readonly string pepper = "nlfk60a9rcse";

        /// <summary>
        /// Hashes passwords with given password, salt, and pepper.
        /// </summary>
        /// <param name="password">The password to be hashed</param>
        /// <param name="salt">The salt used for hashing</param>
        /// <returns>The hashed password</returns>
        public static string HashPasswords(string password, string salt)
        {
            SHA256 sha256 = SHA256Managed.Create();
            string stringToBeHashed = pepper + password + salt;
            byte[] bytes = Encoding.UTF8.GetBytes(stringToBeHashed);
            byte[] hashed = sha256.ComputeHash(bytes);
            string hashedString = "";
            foreach (byte i in hashed)
            {
                hashedString += String.Format("{0:x2}", i);
            }
            return hashedString;
        }
    }
}
