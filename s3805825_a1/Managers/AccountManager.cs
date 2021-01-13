using System;
using System.Collections.Generic;
using System.Linq;
using s3805825_a1.Model;
using s3805825_a1.Utilities;

namespace s3805825_a1.Managers
{
    public class AccountManager
    {
        private readonly string _connectionString;

        public List<Account> Accounts { get; }

        public AccountManager(string connectionString)
        {
            _connectionString = connectionString;
            //using var connection = _connectionString.CreateConnection();
            //var command = connection.CreateCommand();
            //command.CommandText = "select * from Account";

            //var TransactionManager = new TransactionManager(_connectionString);

            //Accounts = command.GetDataTable().Select().Select(x => new Account
            //{
            //    AccountNumber = (int)x["AccountNumber"],
            //    AccountType = (String)x["AccountType"],
            //    Balance = (double)x["Balance money"],
            //    Transactions = TransactionManager.GetTransaction((int)x["AccountNumber"])
            //}).ToList();
            
        }



        public List<Account> GetAccount(int cusID)
        {
            using var connection = _connectionString.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from Account where CustomerID = @cusID";
            command.Parameters.AddWithValue("CustomerID", cusID);
            var TransactionManager = new TransactionManager(_connectionString);
            return command.GetDataTable().Select().Select(x => new Account
            {
                AccountNumber = (int)x["AccountNumber"],
                AccountType = (String)x["AccountType"],
                Balance = (double)x["Balance"],
                Transactions = TransactionManager.GetTransaction((int)x["AccountNumber"])
            }).ToList();
        }

        public void InsertAccount(Account acc, Customer cus)
        {
            using var connection = _connectionString.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                "insert into Account (AccountNumber, AccountType, Balance, CustomerID) values (@AccountNumber, @AccountType,@Balance, @CustomerID)";
            command.Parameters.AddWithValue("AccountNumber", acc.AccountNumber);
            command.Parameters.AddWithValue("AccountType", acc.AccountType);
            command.Parameters.AddWithValue("Balance", acc.Balance);
            command.Parameters.AddWithValue("CustomerID", cus.CustomerID);

            command.ExecuteNonQuery();
        }
    }
}

