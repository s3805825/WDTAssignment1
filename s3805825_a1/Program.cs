using System;
using Microsoft.Extensions.Configuration;

using Assignment_Manager.Managers;
using Assignment_Manager.Services;

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
            DatabaseManager.DropTables(connectionString);
        }
    }
}

