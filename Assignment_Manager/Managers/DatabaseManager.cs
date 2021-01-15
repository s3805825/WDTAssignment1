using System;
using System.IO;
using Assignment_Manager.Utilities;

namespace Assignment_Manager.Managers
{
    public static class DatabaseManager
    {
        public static void CreateTables(string connectionString)
        {
            using var connection = connectionString.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = File.ReadAllText("Sql/CreateTable.sql");

            command.ExecuteNonQuery();
        }

        public static void DropTables(string connectionString)
        {
            using var connection = connectionString.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = File.ReadAllText("Sql/DropTable.sql");

            int numberOfRows = command.ExecuteNonQuery();
            Console.WriteLine("Drop Successful");
        }
    }
}
