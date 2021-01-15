using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Assignment_Manager.Model;
using System.Collections.Generic;
using System.Security.Cryptography;
using SimpleHashing;
using System.Linq;
using Assignment_Manager.Services;
using ConsoleTables;
using Assignment_Manager.Managers;

namespace s3805825_a1
{
    public class Menu
    {
        private readonly CustomerManager customerManager;
        private readonly LoginManager loginAccountManager;
        public Menu(String connectionString)
        {
            customerManager = new CustomerManager(connectionString);
            loginAccountManager = new LoginManager(connectionString);
        }

        public void run()
        {
            while (true)
            {
                int customerID = Login();
                Customer cus = GetCustomer(customerID);
                Boolean exit = ShowMainMenu(cus);
                if (exit == true)
                {
                    return;
                }
            }
        }

        public int Login()
        {
            Console.Clear();
            Console.WriteLine("---- Login Menu ----");
            Console.WriteLine();
            int cusId = 0;
            while (true)
            {
                Console.WriteLine("Please Enter Your Login Id:");
                Console.WriteLine();
                String accountNumber = Console.ReadLine();
                while (accountNumber == "")
                {
                    Console.WriteLine("No Input");
                    Console.WriteLine("Please Enter Your Login Id:");
                    accountNumber = Console.ReadLine();
                }

                Console.WriteLine("Please Enter your Password");
                Console.WriteLine();
                String accountPass = Console.ReadLine();
                while (accountPass == "")
                {
                    Console.WriteLine("No Input");
                    Console.WriteLine("Please Enter Your Password:");
                    accountPass = Console.ReadLine();
                }
                if (ValidLogin(accountNumber, accountPass) == 0)
                {
                    Console.Write("The Login Id or Password provided were incorrect. Please try again.");
                    continue;
                }
                else
                {
                    cusId = ValidLogin(accountNumber, accountPass);
                    break;
                }

            }
            return cusId;

        }
        //check user existence
        //if return 0 means not exist
        public int ValidLogin(String accountNumber, String password)
        {

            foreach (var l in loginAccountManager.Logins)
            {
                if (l.LoginID == accountNumber)
                {
                    if (PBKDF2.Verify(l.PasswordHash, password))
                    {
                        Console.WriteLine("Login Successful");
                        return l.CustomerID;
                    }
                }
            }

            return 0;
        }
        //get customer from memory
        public Customer GetCustomer(int CustomerID)
        {

            foreach (var c in customerManager.Customers)
            {
                if (CustomerID == c.CustomerID)
                {
                    return c;
                }

            }
            return null;
        }



        public Boolean ShowMainMenu(Customer customer)
        {


            while (true)
            {
                Console.Clear();
                Console.WriteLine("---- Main Menu ----");
                Console.WriteLine("");
                Console.WriteLine("Welcome " + customer.Name);
                Console.WriteLine("");
                Console.WriteLine("Please select an option from the following:");
                Console.WriteLine("");
                Console.WriteLine("1. ATM Transaction");
                Console.WriteLine("2. Transfer");
                Console.WriteLine("3. My Statements");
                Console.WriteLine("4. Logout");
                Console.WriteLine("5. Exit");
                var input = Console.ReadLine();
                Console.WriteLine();
                //check if input is int and within range
                if (!int.TryParse(input, out var option) || option > 5 || option < 1)
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                    continue;
                }

                switch (option)
                {
                    case 1:
                        ShowAtmMenu(customer);
                        break;
                    case 2:
                        SelectAccountMenu(3, customer);
                        break;
                    case 3:
                        SelectAccountMenu(4, customer);
                        break;
                    case 4:
                        return false;
                    case 5:
                        return true;
                    default:
                        throw new InvalidOperationException();
                }
            }

        }

        public void ShowAtmMenu(Customer customer)
        {
            Console.Clear();
            Console.WriteLine("---- ATM Menu ----");
            Console.WriteLine("");
            Console.WriteLine("Please select an option from the following:");
            Console.WriteLine("");
            Console.WriteLine("1. Deposit Money");
            Console.WriteLine("2. Withdraw Money");
            Console.WriteLine("3. Return to Main Menu");
            while (true)
            {
                var input = Console.ReadLine();
                Console.WriteLine();
                if (!int.TryParse(input, out var option) || option > 3 || option < 1)
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                    continue;
                }

                switch (option)
                {
                    case 1:
                        SelectAccountMenu(1, customer); return;
                    case 2:
                        SelectAccountMenu(2, customer); return;
                    case 3:
                        return;
                    default:
                        throw new InvalidOperationException();
                }
            }

        }

        public void ShowDepositMenu(Customer customer, Account acc)
        {
            Console.Clear();
            Console.WriteLine("---- Deposit Amount ----");
            Console.WriteLine("");
            Console.WriteLine("Your available balance is : " + acc.Balance);
            Console.WriteLine("");
            Console.WriteLine("Enter the amount you would like to deposit, or press enter to return: ");
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "") return;
                Console.WriteLine();
                
                if (!IsMoney(input)|| GetMoney(input) < 0)
                {
                    Console.WriteLine("Invalid input. Try Again or enter to quit");
                    Console.WriteLine();
                    continue;
                }
                else
                {
                    Console.WriteLine("Deposit of " + input + " is successful");
                    
                    customer.Deposit(acc, GetMoney(input),GetTransactionNumber());
                    Console.WriteLine("");
                    Console.WriteLine("Press any key to return to the account selection menu");
                    Console.ReadLine();
                    return;
                }
            }

        }

        public Boolean IsMoney(String s)
        {
            if (!int.TryParse(s, out var option))
            {
                if (!double.TryParse(s, out var soption))
                {
                    return false;
                }
                
            }
            return true;
        }

        public double GetMoney(String s)
        {
            if (int.TryParse(s, out var option))
            {
                double vOut = Convert.ToDouble(s);
                return vOut;
            }
            if (double.TryParse(s, out var soption))
            {
                return soption;
            }
            return 0.0;
        }

        public void ShowWithdrawMenu(Customer customer, Account acc)
        {
            Console.Clear();
            Console.WriteLine("---- Withdraw Amount ----");
            Console.WriteLine("");
            Console.WriteLine("Your available balance is : " + acc.Balance);
            Console.WriteLine("");
            Console.WriteLine("Enter the amount you would like to withdraw, or press enter to return: ");
            while (true)
            {
                
                
                var input = Console.ReadLine();
                if (input == "") return;
                Console.WriteLine();
                if (!IsMoney(input) || GetMoney(input) < 0)
                {
                    Console.WriteLine("Invalid input, or press enter to return: ");
                    Console.WriteLine();
                    continue;
                }
                else
                {
                    Console.WriteLine("Withdraw of " + input + " is successful");
                    //Withdraw(customer, acc, int.Parse(input));
                    if (!customer.Withdraw(acc, GetMoney(input),GetTransactionNumber()))
                    {
                        continue;
                    }
                    Console.WriteLine("");
                    Console.WriteLine("Press any key to return to the account selection menu");
                    Console.ReadLine();
                    return;

                }
            }

        }

        public void ShowTransferMenu(Customer customer, Account acc)
        {
            Console.Clear();
            Console.WriteLine("---- Transfer Menu ----");
            Console.WriteLine("");
            Console.WriteLine("Enter the account number you wish to transfer to, or press enter to return to the account election menu:");

            while (true)
            {

                
                
                var transferToAcc = Console.ReadLine();
                if (transferToAcc == "") return;
                if (int.Parse(transferToAcc) == acc.AccountNumber)
                {
                    Console.WriteLine("You should not transfer to the same account, or press enter to return: ");
                    continue;
                }
                if (!int.TryParse(transferToAcc, out var choice) || ExistTransferToAccount(int.Parse(transferToAcc)) == null)
                {
                    Console.WriteLine("Invalid input or This Account doesn't exist, or press enter to return: ");
                    continue;
                }
                else
                {
                    var to = ExistTransferToAccount(int.Parse(transferToAcc));
                    Console.WriteLine("Your available balance is : " + acc.Balance);
                    Console.WriteLine("");
                    Console.WriteLine("Enter the amount you would like to transfer to account " + to.AccountNumber + ", or press enter to return: ");
                    var input = Console.ReadLine();
                    if (input == "") return;
                    Console.WriteLine();
                    if (!IsMoney(input) || GetMoney(input) < 0)
                    {
                        Console.WriteLine("Invalid input.");
                        Console.WriteLine();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Please add a description(Optional) :");
                        var des = Console.ReadLine();

                        //TransferTo(customer, acc, int.Parse(input));
                        if (!customer.TransferTo(acc, to, GetMoney(input), des, GetTransactionNumber())) continue;
                        Console.WriteLine("Transfer of " + input + " is successful");
                        Console.WriteLine("");
                        Console.WriteLine("Press any key to return to the account selection menu");
                        Console.ReadLine();
                        return;

                    }
                }

                
            }

        }

        Account ExistTransferToAccount(int AccNumber)
        {
            foreach (var c in customerManager.Customers)
            {
                foreach (var accc in c.Accounts)
                {
                    if (AccNumber == accc.AccountNumber)
                    {
                        return accc;                       
                    }
                }
            }
            return null;
        }

        public static void ShowDisplayMenu(Customer customer, Account acc)
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Account " + acc.AccountNumber + " - Current balance: $" + acc.Balance);
                Console.WriteLine();

                var table = new ConsoleTable("Transaction Id", "Type", "From Account", "To Account", "Amount", "Comment", "Date");
                foreach (var trans in acc.Transactions)
                {
                    if (trans.TransactionTo == 0)
                    {
                        table.AddRow(trans.TransactionID, trans.TransactionType, trans.TransactionFrom, " ", trans.Amount, trans.Comment, trans.TransactionTimeUtc);
                    }
                    else
                    {
                        table.AddRow(trans.TransactionID, trans.TransactionType, trans.TransactionFrom, trans.TransactionTo, trans.Amount, trans.Comment, trans.TransactionTimeUtc);
                    }
                    
                }
                table.Write();
                Console.WriteLine("Press enter to return");
                Console.ReadLine();
                return;
            }

        }

        public int GetTransactionNumber()
        {
            int i = 0;
            foreach (var c in customerManager.Customers)
            {
                foreach (var acc in c.Accounts)
                {
                    foreach (var t in acc.Transactions)
                    {
                        i++;
                    }
                }
            }
            return i;
        }

        public void SelectAccountMenu(int a, Customer customer)
        {
            Console.Clear();
            Console.WriteLine("---- Select Account ----");
            Console.WriteLine("");
            if (a == 1)
            {
                Console.WriteLine("Select an Account to deposit money into: ");
            }
            else if (a == 2)
            {
                Console.WriteLine("Select an Account to Withdraw money from: ");
            }
            else if (a == 3)
            {
                Console.WriteLine("Select an Account to transfer money from: ");
            }
            else if (a == 4)
            {
                Console.WriteLine("Select an Account to display statement for: ");
            }
            Console.WriteLine("");
            Account s = customer.GetAccountByType("S");
            Account c = customer.GetAccountByType("C");
            int limit = 3;
            if (s == null)
            {
                Console.WriteLine("1. Checking Account " + c.AccountNumber);
                Console.WriteLine("2. Return to Main Menu");
                limit = 2;
            }
            else if (c == null)
            {
                Console.WriteLine("1. Savings Account " + s.AccountNumber);
                Console.WriteLine("2. Return to Main Menu");
                limit = 2;
            }
            else
            {
                Console.WriteLine("1. Savings Account " + s.AccountNumber);
                Console.WriteLine("2. Checking Account " + c.AccountNumber);
                Console.WriteLine("3. Return to Main Menu");
            }


            while (true)
            {

                var input = Console.ReadLine();

                Console.WriteLine();
                if (!int.TryParse(input, out var option) || option > limit || option < 1)
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                    continue;
                }

                if (a == 1)
                {
                    if (s == null)
                    {
                        switch (option)
                        {
                            case 1:
                                ShowDepositMenu(customer, c); return;
                            case 2:
                                return;
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                    else if (c == null)
                    {
                        switch (option)
                        {
                            case 1:
                                ShowDepositMenu(customer, s); return;
                            case 2:
                                return;
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                    else
                    {
                        switch (option)
                        {
                            case 1:
                                ShowDepositMenu(customer, s); return;
                            case 2:
                                ShowDepositMenu(customer, c); return;
                            case 3:
                                return;
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                    
                }
                else if (a == 2)
                {
                    if (s == null)
                    {
                        switch (option)
                        {
                            case 1:
                                ShowWithdrawMenu(customer, c); return;
                            case 2:
                                return;
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                    else if (c == null)
                    {
                        switch (option)
                        {
                            case 1:
                                ShowWithdrawMenu(customer, s); return;
                            case 2:
                                return;
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                    else
                    {
                        switch (option)
                        {
                            case 1:
                                ShowWithdrawMenu(customer, s); return;
                            case 2:
                                ShowWithdrawMenu(customer, c); return;
                            case 3:
                                return;
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                    
                }
                else if (a == 3)
                {
                    if (s == null)
                    {
                        switch (option)
                        {
                            case 1:
                                ShowTransferMenu(customer, c); return;
                            case 2:
                                return;
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                    else if (c == null)
                    {
                        switch (option)
                        {
                            case 1:
                                ShowTransferMenu(customer, s); return;
                            case 2:
                                return;
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                    else
                    {
                        switch (option)
                        {
                            case 1:
                                ShowTransferMenu(customer, s); return;
                            case 2:
                                ShowTransferMenu(customer, c); return;
                            case 3:
                                return;
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                    
                }
                else if (a == 4)
                {
                    if (s == null)
                    {
                        switch (option)
                        {
                            case 1:
                                ShowDisplayMenu(customer, c); return;
                            case 2:
                                return;
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                    else if (c == null)
                    {
                        switch (option)
                        {
                            case 1:
                                ShowDisplayMenu(customer, s); return;
                            case 2:
                                return;
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                    else
                    {
                        switch (option)
                        {
                            case 1:
                                ShowDisplayMenu(customer, s); return;
                            case 2:
                                ShowDisplayMenu(customer, c); return;
                            case 3:
                                return;
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                    
                }

            }

        }
    }
}
