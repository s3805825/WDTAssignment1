using System;
using System.Collections.Generic;
using System.Linq;
using Assignment_Manager.Model;
using Assignment_Manager.Utilities;

namespace Assignment_Manager.Managers
{
    public class CustomerManager
    {
        private readonly string _connectionString;
        public List<Customer> Customers { get; }
        public CustomerManager(String connectionString)
        {
            _connectionString = connectionString;

            using var connection = _connectionString.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from Customer";
            var AccountManager = new AccountManager(_connectionString);
            Customers = command.GetDataTable().Select().Select(x => new Customer
            {
                CustomerID = (int)x["CustomerID"],
                Name = (string)x["Name"],
                Address = IfNullInDatabase(x["Address"]),
                City = IfNullInDatabase(x["City"]),
                PostCode = IfNullInDatabase(x["PostCode"]),
                Accounts = AccountManager.GetAccount((int)x["CustomerID"])
            }).ToList();
        }
        public void InsertCustomer(Customer customer)
        {
            using var connection = _connectionString.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                "insert into Customer (CustomerID, Name, Address, City, PostCode) values (@CustomerID, @Name, @Address,@City, @PostCode)";
            command.Parameters.AddWithValue("CustomerID", customer.CustomerID);
            command.Parameters.AddWithValue("Name", customer.Name);
            command.Parameters.AddWithValue("Address", IfNull(customer.Address));
            command.Parameters.AddWithValue("City", IfNull(customer.City));
            command.Parameters.AddWithValue("PostCode", IfNull(customer.PostCode));
            command.ExecuteNonQuery();
        }

        Object IfNull(String ob)
        {
            if (ob == null)
                return DBNull.Value;

            return ob;
        }

        string IfNullInDatabase(object ob)
        {
            if (ob == DBNull.Value)
                return string.Empty;
            return ob.ToString();
        }
    }
}

