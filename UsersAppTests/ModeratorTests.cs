using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsersApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using UsersApp.Properties;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace UsersApp.Tests
{
    [TestClass()]
    public class ModeratorTests
    {
        [TestMethod()]
        public void ConnectionString_Read()
        {
            Settings settings = new Settings();
            Assert.IsFalse(string.IsNullOrWhiteSpace(settings.DemoConnectionString));
        }

        [TestMethod()]
        public void ConnectionString_Check()
        {
            Settings settings = new Settings();
            SqlConnection connection = new SqlConnection(settings.DemoConnectionString);
            connection.Open();
            Assert.IsTrue(connection.State == ConnectionState.Open);
        }
    }
}