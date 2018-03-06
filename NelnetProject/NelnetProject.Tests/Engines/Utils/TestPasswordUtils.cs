using Engines.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NelnetProject.Tests.Engines.Utils
{
    [TestClass]
    public class TestPasswordUtils
    {
        [TestMethod]
        public void TestHashPasswords()
        {
            string password = "password";
            string salt = "l1u2c3a4s5";
            string testHash = "78b10e2cb3ec22bffea25bad2a1c02cbe4b7b587b46d0dd8d6af1c170910a3b1";
            Assert.AreEqual(PasswordUtils.HashPasswords(password, salt), testHash);
        }
    }
}