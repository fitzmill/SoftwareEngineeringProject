using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

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
