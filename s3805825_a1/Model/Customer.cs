using System;
using System.Collections.Generic;

namespace s3805825_a1.Model
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public String Name { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String PostCode { get; set; }
        public List<Account> Accounts { get; set; }
        public int FreeTransferAllowance;
        public Customer()
        {
        }

        public static Account GetAccountByType(String type, Customer customer)
        {
            foreach (var acc in customer.Accounts)
            {
                if (type == acc.AccountType)
                {
                    return acc;
                }
            }
            return null;
        }

        public void Deposit(Account acc, int amount)
        {
            foreach (var account in Accounts)
            {
                if (acc == account)
                {
                    account.Balance += amount;

                    Transactions t = new Transactions();
                    t.TransactionTimeUtc = DateTime.Now.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss tt");
                    t.Amount = amount;
                    t.TransactionID = 1;
                    t.TransactionTo = acc.AccountNumber;
                    t.TransactionType = "D";
                    t.Comment = "Deposit";
                    account.Transactions.Add(t);
                }
            }


        }

        public void Withdraw(Account acc, int amount)
        {
            foreach (var account in Accounts)
            {
                if (acc == account)
                {
                    account.Balance -= amount;


                    Transactions t = new Transactions();
                    t.TransactionTimeUtc = DateTime.Now.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss tt");
                    t.Amount = amount;
                    t.TransactionID = 1;
                    t.TransactionFrom = acc.AccountNumber;
                    t.TransactionType = "W";
                    t.Comment = "Withdraw";
                    account.Transactions.Add(t);
                }
            }
        }

        public void TransferTo(Account acc, int amount, String des)
        {

            var from = new Account();
            Transactions t = new Transactions();
            t.TransactionTimeUtc = DateTime.Now.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss tt");
            foreach (var account in Accounts)
            {
                if (acc == account)
                {
                    account.Balance += amount;
                }
                else
                {
                    from = account;
                    account.Balance -= amount;
                }


            }
            t.Amount = amount;
            t.TransactionID = 1;
            t.TransactionFrom = from.AccountNumber;
            t.TransactionTo = acc.AccountNumber;
            t.TransactionType = "T";
            t.Comment = des;

            foreach (var acco in Accounts)
            {
                acco.Transactions.Add(t);
            }

        }

        public Account GetAccountByType(String type)
        {
            foreach (var acc in Accounts)
            {
                if (type == acc.AccountType)
                {
                    return acc;
                }
            }
            return null;
        }
    }
}

