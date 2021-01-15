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

        public Account GetAccountByType(string s)
        {
            foreach (var acc in Accounts)
            {
                if (acc.AccountType == s)
                    return acc;
            }
            return null;
        }

        public void Deposit(Account acc, double amount, int TransID)
        {
            foreach (var account in Accounts)
            {
                if (acc == account)
                {
                    //Increase Account Balance
                    account.Balance += amount;

                    Transactions t = new Transactions();
                    t.TransactionID = TransID;
                    //Get Current Time
                    t.TransactionTimeUtc = DateTime.Now.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss tt");
                    t.Amount = amount;
                    
                    t.TransactionFrom = acc.AccountNumber;
                    t.TransactionType = "D";
                    t.Comment = "Deposit";
                    account.Transactions.Add(t);
                }
            }


        }

        public Boolean Withdraw(Account acc, Double amount, int TransID)
        {
            double serviceFee = 0;
            //if the free allowance is used, service fee will be charged
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
                        t.TransactionID = TransID;
                        t.TransactionFrom = acc.AccountNumber;
                        t.TransactionType = "W";
                        t.Comment = "Withdraw";
                        account.Transactions.Add(t);
                        //service transaction recorded
                        if (FreeTransferAllowance < 1)
                        {
                            Transactions service = new Transactions();
                            service.TransactionID = TransID+1;
                            //Requires the same time of withdraw
                            service.Amount = 0.1;
                            service.TransactionType = "S";
                            service.TransactionFrom = acc.AccountNumber;
                            service.TransactionTimeUtc = t.TransactionTimeUtc;
                            service.Comment = "Withdraw Service Fee";
                            account.Transactions.Add(service);
                        }
                        else
                        {
                            //Free allowance decrease when there's still allowance
                            FreeTransferAllowance -= 1;
                        }
                        return true;
                    }
                }
            }

            return false;
        }

        public Boolean TransferTo(Account From, Account acc, Double amount, String des, int TransID)
        {
            double serviceFee = 0;

            if (FreeTransferAllowance < 1)
            {
                serviceFee = 0.2;
            }
            //check is balance is allowed.
            if (From.AccountType == "S" && From.Balance - amount - serviceFee > 0 || From.AccountType == "C" && From.Balance - amount - serviceFee > 200)
            {
                
                From.Balance = From.Balance - amount - serviceFee;
                Transactions t = new Transactions();
                t.TransactionTimeUtc = DateTime.Now.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss tt");
                t.Amount = amount;
                t.TransactionID = TransID;
                t.TransactionFrom = From.AccountNumber;
                t.TransactionTo = acc.AccountNumber;
                t.TransactionType = "T";
                t.Comment = des;
                From.Transactions.Add(t);
                acc.Balance += amount;
                acc.Transactions.Add(t);

                if (FreeTransferAllowance < 1)
                {
                    Transactions service = new Transactions();
                    service.TransactionID = TransID + 1;
                    //Requires the same time of withdraw
                    service.Amount = 0.1;
                    service.TransactionType = "S";
                    service.TransactionFrom = From.AccountNumber;
                    service.TransactionTimeUtc = t.TransactionTimeUtc;
                    service.Comment = "Withdraw Service Fee";
                    From.Transactions.Add(service);
                }
                else
                {
                    FreeTransferAllowance -= 1;
                }
                Console.WriteLine("Transfer Successful");
                return true;
            }
            Console.WriteLine("Insufficient Balance Transfer failed. Try again");
            return false;
            

        }

    }
}

