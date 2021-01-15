using System;
using System.Collections.Generic;
using System.Linq;
using Assignment_Manager.Model;
using Assignment_Manager.Utilities;

namespace Assignment_Manager.Managers
{
    public class LoginManager
    {
        private readonly string _connectionString;

        public List<LoginAccount> Logins { get; }

        public LoginManager(string connectionString)
        {
            _connectionString = connectionString;

            using var connection = _connectionString.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from Login";

            var cusManager = new CustomerManager(_connectionString);

            Logins = command.GetDataTable().Select().Select(x => new LoginAccount
            {
                LoginID = (string)x["LoginID"],
                CustomerID = (int)x["CustomerID"],
                PasswordHash = (string)x["PasswordHash"]
            }).ToList();
        }

        public void InsertLogin(LoginAccount login)
        {
            using var connection = _connectionString.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                "insert into Login (LoginID, CustomerID, PasswordHash) values (@loginID, @cus, @pass)";
            command.Parameters.AddWithValue("loginID", login.LoginID);
            command.Parameters.AddWithValue("cus", login.CustomerID);
            command.Parameters.AddWithValue("pass", login.PasswordHash);

            command.ExecuteNonQuery();
        }
    }
}
