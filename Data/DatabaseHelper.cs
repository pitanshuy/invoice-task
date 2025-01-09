//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
namespace invoice_task.Data
{
    public class DatabaseHelper
    {

      


   
            private readonly string _connectionString;

            public DatabaseHelper(string connectionString)
            {
                _connectionString = connectionString;
            }

            public SqlConnection GetConnection()
            {
                var connection = new SqlConnection(_connectionString);
                connection.Open();
                return connection;
            }
        }
    }



