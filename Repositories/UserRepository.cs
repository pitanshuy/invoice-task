//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

using System.Threading.Tasks;
using invoice_task.Data;
using invoice_task.Domain.Entities;
using invoice_task.Interfaces;
using System.Data;
using System;

namespace invoice_task.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseHelper _dbHelper;

        // Constructor to inject DatabaseHelper
        public UserRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        //public User GetUserByUsername(string username)
        //{
        //    using (var connection = _dbHelper.GetConnection())
        //    {
        //        connection.Open();
        //        using (var command = new SqlCommand("SELECT * FROM Users WHERE Username = @Username", connection))
        //        {
        //            command.Parameters.AddWithValue("@Username", username);
        //            using (var reader = command.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    return new User
        //                    {
        //                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
        //                        Username = reader.GetString(reader.GetOrdinal("Username")),
        //                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
        //                        Role = reader.GetString(reader.GetOrdinal("Role"))
        //                    };
        //                }
        //            }
        //        }
        //    }
        //    return null;
        //}

        public User GetUserByUsername(string username)
        {
            SqlConnection connection = null;
            try
            {
                connection = _dbHelper.GetConnection();
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                connection.Open();
                using (var command = new SqlCommand("SELECT Id,Email as  Username,Password as PasswordHash, Role FROM Users WHERE Email = @Username", connection))
                {
                    command.Parameters.Add("@Username", SqlDbType.NVarChar, 255).Value = username;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                //Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Id = reader.GetInt32(reader.GetOrdinal("Id")), // Change to GetInt32

                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                                Role = reader.GetString(reader.GetOrdinal("Role"))
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log or handle the exception
                throw new Exception("An error occurred while retrieving the user data.", ex);
            }
            finally
            {
                // Ensure the connection is closed if it’s open
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return null;
        }



        public async Task<User> GetByEmailAsync(string email)
        {
            User user = null;

            using (var connection = _dbHelper.GetConnection())
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("SELECT * FROM Users WHERE Email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new User
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                                Role = reader.GetString(reader.GetOrdinal("Role"))
                            };
                        }
                    }
                }
            }

            return user;
        }
    }
}
