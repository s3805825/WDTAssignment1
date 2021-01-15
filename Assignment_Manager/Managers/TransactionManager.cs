using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Assignment_Manager.Model;
using Assignment_Manager.Utilities;

namespace Assignment_Manager.Managers
{
    public class TransactionManager
    {
        private readonly string _connectionString;

        public TransactionManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Transactions> GetTransaction(int AccountNumber)
        {
            using var connection = _connectionString.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from [transaction] where AccountNumber = @accID or DestinationAccountNumber = @daccID";
            command.Parameters.AddWithValue("accID", AccountNumber);
            command.Parameters.AddWithValue("daccID", AccountNumber);

            return command.GetDataTable().Select().Select(x => new Transactions
            {
                TransactionID = (int)x["TransactionID"],
                TransactionTimeUtc = Convert.ToDateTime(x["TransactionTimeUtc"]).ToString("dd/MM/yyyy HH:mm:ss tt"),
                TransactionType = (string)x["TransactionType"],
                TransactionFrom = (int)x["AccountNumber"],
                Comment = (string)x["Comment"],
                Amount = (double)(decimal)x["Amount"]
            }).ToList();
        }

        public void InsertTransaction(Transactions Trans, Account acc)
        {
            using var connection = _connectionString.CreateConnection();
            connection.Open();
            char word = 'D';
            var command = connection.CreateCommand();
            command.CommandText =
                "insert into [transaction] (TransactionType, AccountNumber, DestinationAccountNumber, Amount,Comment, TransactionTimeUtc) values " +
                "( @type, @from, @to, @amount,@comment, @time)";
            //command.Parameters.AddWithValue("tranID", GetTransactionID());
            command.Parameters.AddWithValue("type", word);
            command.Parameters.AddWithValue("from", acc.AccountNumber);
            command.Parameters.AddWithValue("to", DBNull.Value);
            command.Parameters.AddWithValue("amount", acc.Balance);
            command.Parameters.AddWithValue("comment", "");
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "dd/MM/yyyy HH:mm:ss tt";
            command.Parameters.AddWithValue("time", Convert.ToDateTime(Trans.TransactionTimeUtc, dtFormat));
            
            command.ExecuteNonQuery();
        }
        
        
        
    }
}
