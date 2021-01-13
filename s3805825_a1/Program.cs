using System;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using s3805825_a1.Model;
using System.Collections.Generic;
using System.Security.Cryptography;
using SimpleHashing;
using System.Linq;
using s3805825_a1.Managers;
using s3805825_a1.Services;

namespace s3805825_a1
{
    class MainClass
    {
        public static void Main()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = configuration["ConnectionString"];
            DatabaseManager.CreateTables(connectionString);
            CustomerWebService.DataStoreProcess(connectionString);
            new Menu(connectionString).run();
        }

        public static void MainZZ(String[] args)
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


        public static int Login()
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

        public static int ValidLogin(String accountNumber, String password)
        {

            using var client = new HttpClient();
            var url = "https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/logins/";
            var json = client.GetStringAsync(url).Result;

            var loginAcc = JsonConvert.DeserializeObject<List<LoginAccount>>(json);

            foreach (var l in loginAcc)
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

        public static Customer GetCustomer(int CustomerID)
        {
            Customer customer = new Customer();
            using var client = new HttpClient();

            var url = "https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/";
            var json = client.GetStringAsync(url).Result;

            var customerList = JsonConvert.DeserializeObject<List<Customer>>(json, new JsonSerializerSettings
            {
                DateFormatString = "dd/MM/yyyy HH:mm:ss aa"
            });

            foreach (var c in customerList)
            {
                if (CustomerID == c.CustomerID)
                {
                    customer = c;
                }

            }
            return customer;
        }


        public static Boolean ShowMainMenu(Customer customer)
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

        public static void ShowAtmMenu(Customer customer)
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine("---- ATM Menu ----");
                Console.WriteLine("");
                Console.WriteLine("Please select an option from the following:");
                Console.WriteLine("");
                Console.WriteLine("1. Deposit Money");
                Console.WriteLine("2. Withdraw Money");
                Console.WriteLine("3. Return to Main Menu");
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

        public static void ShowDepositMenu(Customer customer, Account acc)
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine("---- Deposit Amount ----");
                Console.WriteLine("");
                Console.WriteLine("Your available balance is : " + acc.Balance);
                Console.WriteLine("");
                Console.WriteLine("Enter the amount you would like to deposit, or press enter to return: ");
                var input = Console.ReadLine();
                if (input == "") return;
                Console.WriteLine();
                if (input == "") return;
                if (!int.TryParse(input, out var option) || option < 0)
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                    continue;
                }
                else
                {
                    Console.WriteLine("Deposit of " + input + " is successful");
                    //Deposit(customer, acc, int.Parse(input));
                    customer.Deposit(acc, int.Parse(input));
                    Console.WriteLine("");
                    Console.WriteLine("Press any key to return to the account selection menu");
                    Console.ReadLine();
                    return;
                }
            }

        }

        public static void ShowWithdrawMenu(Customer customer, Account acc)
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine("---- Withdraw Amount ----");
                Console.WriteLine("");
                Console.WriteLine("Your available balance is : " + acc.Balance);
                Console.WriteLine("");
                Console.WriteLine("Enter the amount you would like to withdraw, or press enter to return: ");
                var input = Console.ReadLine();
                if (input == "") return;
                Console.WriteLine();
                if (!int.TryParse(input, out var option) || option < 0 || option > acc.Balance)
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                    continue;
                }
                else
                {
                    Console.WriteLine("Withdraw of " + input + " is successful");
                    //Withdraw(customer, acc, int.Parse(input));
                    customer.Withdraw(acc, int.Parse(input));
                    Console.WriteLine("");
                    Console.WriteLine("Press any key to return to the account selection menu");
                    Console.ReadLine();
                    return;

                }
            }

        }

        public static void ShowTransferMenu(Customer customer, Account acc)
        {

            while (true)
            {
                var from = new Account();
                foreach (var account in customer.Accounts)
                {
                    if (account != acc)
                    {
                        from = account;
                    }
                }
                Console.Clear();
                Console.WriteLine("---- Transfer Amount ----");
                Console.WriteLine("");
                Console.WriteLine("Your available balance is : " + from.Balance);
                Console.WriteLine("");
                Console.WriteLine("Enter the amount you would like to transfer to account " + acc.AccountNumber + ", or press enter to return: ");

                var input = Console.ReadLine();
                if (input == "") return;
                Console.WriteLine();
                if (!int.TryParse(input, out var option) || option < 0 || option > from.Balance)
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
                    customer.TransferTo(acc, int.Parse(input), des);
                    Console.WriteLine("Transfer of " + input + " is successful");
                    Console.WriteLine("");
                    Console.WriteLine("Press any key to return to the account selection menu");
                    Console.ReadLine();
                    return;

                }
            }

        }

        public static void ShowDisplayMenu(Customer customer, Account acc)
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Account " + acc.AccountNumber + " - Current balance: $" + acc.Balance);
                Console.WriteLine("");
                Console.WriteLine(
@"__________________________________________________________________________________________
|Transaction Id|Type|From Account|To Account|   Amount|          Comment|              Date|
|--------------|----|------------|----------|---------|-----------------|------------------|");
                foreach (var b in acc.Transactions)
                {
                    Console.WriteLine(
@"|         {0}| {1}|          {2}|       {3}|      {4}|             {5}|{6}               |
|--------------|----|------------|----------|---------|-----------------|------------------|
", b.TransactionID, b.TransactionType, b.TransactionFrom, b.TransactionTo, b.Amount, b.Amount, b.TransactionTimeUtc);
                }

                Console.WriteLine(
@"_________________________________________________________________________________________
");

                Console.WriteLine("Press enter to return");
                Console.ReadLine();
                return;
            }

        }

        public static void SelectAccountMenu(int a, Customer customer)
        {


            while (true)
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
                Console.WriteLine("1. Savings Account " + s.AccountNumber);
                Console.WriteLine("2. Checking Account " + c.AccountNumber);
                Console.WriteLine("3. Return to Main Menu");
                var input = Console.ReadLine();

                Console.WriteLine();
                if (!int.TryParse(input, out var option) || option > 3 || option < 1)
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                    continue;
                }
                if (a == 1)
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
                else if (a == 2)
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
                else if (a == 3)
                {
                    switch (option)
                    {
                        case 1:
                            ShowTransferMenu(customer, c); return;
                        case 2:
                            ShowTransferMenu(customer, s); return;
                        case 3:
                            return;
                        default:
                            throw new InvalidOperationException();
                    }
                }
                else if (a == 4)
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
