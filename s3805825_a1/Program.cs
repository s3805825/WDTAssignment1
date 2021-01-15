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
            //create table first
            DatabaseManager.CreateTables(connectionString);
            //get data from database and insert into memory
            CustomerWebService.DataStoreProcess(connectionString);
            new Menu(connectionString).run();
            //drop table from database 
            DatabaseManager.DropTables(connectionString);
        }
    }
}

