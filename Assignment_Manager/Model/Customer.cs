using System;
using System.Collections.Generic;

namespace Assignment_Manager.Model
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
            FreeTransferAllowance = 4;
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

        public void Deposit(Account acc, double amount)
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
                    t.TransactionFrom = acc.AccountNumber;
                    t.TransactionType = "D";
                    t.Comment = "Deposit";
                    account.Transactions.Add(t);
                }
            }


        }

        public Boolean Withdraw(Account acc, Double amount)
        {
            double serviceFee = 0;
            
            if (FreeTransferAllowance < 1)
            {
                serviceFee = 0.1;
            }
            if (acc.AccountType == "S" && acc.Balance - amount - serviceFee> 0 || acc.AccountType == "C" && acc.Balance - amount - serviceFee > 200)
            {
                foreach (var account in Accounts)
                {
                    if (acc == account)
                    {
                        acc.Balance = acc.Balance - serviceFee - amount;
                        Transactions t = new Transactions();
                        t.TransactionTimeUtc = DateTime.Now.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss tt");
                        t.Amount = amount;
                        t.TransactionID = 1;
                        t.TransactionFrom = acc.AccountNumber;
                        t.TransactionType = "W";
                        t.Comment = "Withdraw";
                        account.Transactions.Add(t);

                        if (FreeTransferAllowance < 1)
                        {
                            t.Amount = 0.1;
                            t.TransactionType = "S";
                            t.Comment = "Withdraw Service Fee";
                            account.Transactions.Add(t);
                        }
                        return true;
                    }
                }
            }

            return false;
        }

        public Boolean TransferTo(Account From, Account acc, Double amount, String des)
        {
            double serviceFee = 0;

            if (FreeTransferAllowance < 1)
            {
                serviceFee = 0.2;
            }
            if (From.AccountType == "S" && From.Balance - amount - serviceFee > 0 || From.AccountType == "C" && From.Balance - amount - serviceFee > 200)
            {
                From.Balance = From.Balance - amount - serviceFee;
                Transactions t = new Transactions();
                t.TransactionTimeUtc = DateTime.Now.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss tt");
                t.Amount = amount;
                t.TransactionID = 1;
                t.TransactionFrom = From.AccountNumber;
                t.TransactionTo = acc.AccountNumber;
                t.TransactionType = "T";
                t.Comment = des;
                From.Transactions.Add(t);

                acc.Balance += amount;

                acc.Transactions.Add(t);

                if (FreeTransferAllowance < 1)
                {
                    t.Amount = 0.1;
                    t.TransactionType = "S";
                    t.Comment = "Transfer Service Fee";
                    From.Transactions.Add(t);
                }
                Console.WriteLine("Transfer Successful");
                return true;
            }
            Console.WriteLine("Insufficient Balance Transfer failed. Try again");
            return false;
            

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

