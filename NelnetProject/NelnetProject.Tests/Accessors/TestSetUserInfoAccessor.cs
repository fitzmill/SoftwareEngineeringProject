using Core;
using Accessors;
using Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace NelnetProject.Tests.Accessors
{
    [TestClass]
    public class TestSetUserInfoAccessor
    {
        ISetUserInfoAccessor setUserInfoAccessor;
        string connectionString;

        public TestSetUserInfoAccessor()
        {
            connectionString = ConfigurationManager.ConnectionStrings["NelnetPaymentProcessing"].ConnectionString;
            setUserInfoAccessor = new SetUserInfoAccessor(connectionString);
        }

        [TestMethod]
        public void TestConnection()
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Close();
            }
        }
    }
}
