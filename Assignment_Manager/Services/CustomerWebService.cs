using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Assignment_Manager.Managers;
using Assignment_Manager.Model;

namespace Assignment_Manager.Services
{
    public static class CustomerWebService
    {
        public static void DataStoreProcess(string connectionString)
        {
            // Check if any people already exist and if they do stop.
            var logManager = new LoginManager(connectionString);
            if (logManager.Logins.Any())
                return;

            // Contact webservice.
            using var client = new HttpClient();
            var json =
                client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/logins/").Result;

            // Convert JSON into objects.
            var logins = JsonConvert.DeserializeObject<List<LoginAccount>>(json);

            // Insert into database.

            var url = "https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/";
            json = client.GetStringAsync(url).Result;

            var customerList = JsonConvert.DeserializeObject<List<Customer>>(json, new JsonSerializerSettings
            {
                DateFormatString = "dd/MM/yyyy HH:mm:ss aa"
            });

            var cusManager = new CustomerManager(connectionString);
            var accManager = new AccountManager(connectionString);
            var tranManager = new TransactionManager(connectionString);
            
            foreach (var c in customerList)
            {
                cusManager.InsertCustomer(c);
                foreach (var acc in c.Accounts)
                {
                    accManager.InsertAccount(acc, c);
                    foreach (var t in acc.Transactions)
                    {
                        tranManager.InsertTransaction(t,acc);
                    }

                }
            }
            foreach (var l in logins)
            {
                logManager.InsertLogin(l);

            }
        }
    }
}
