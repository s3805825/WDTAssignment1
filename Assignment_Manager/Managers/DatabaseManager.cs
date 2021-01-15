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
    }
}
