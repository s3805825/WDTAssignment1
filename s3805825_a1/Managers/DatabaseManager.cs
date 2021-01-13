using System.IO;
using s3805825_a1.Utilities;

namespace s3805825_a1.Managers
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
