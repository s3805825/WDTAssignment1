using System.Data;
using Microsoft.Data.SqlClient;
namespace Assignment_Manager.Utilities
{
    public static class ExtensionUtilities
    {
        public static bool IsInRange(this int value, int min, int max) => value >= min && value <= max;

        public static SqlConnection CreateConnection(this string connectionString) => new SqlConnection(connectionString);

        public static DataTable GetDataTable(this SqlCommand command)
        {
            var table = new DataTable();
            new SqlDataAdapter(command).Fill(table);

            return table;
        }
    }
}
