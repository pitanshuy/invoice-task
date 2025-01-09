using System;
using System.Collections.Generic;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

using invoice_task.Interfaces;
using invoice_task.Data;
using invoice_task.Domain.Entities;
using invoice_task.DTO;
using static invoice_task.Domain.Entities.Invoice;
using System.Data;

namespace invoice_task.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {

        private readonly DatabaseHelper _dbHelper;

        public InvoiceRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        //public IEnumerable<Invoice> GetInvoices()
        //{
        //    var invoices = new List<Invoice>();
        //    using (var connection = _dbHelper.GetConnection())
        //    {
        //        using (var command = new SqlCommand("SELECT * FROM Invoices", connection))
        //        {
        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    invoices.Add(new Invoice
        //                    {
        //                        Id = reader.GetGuid("Id"),
        //                        CustomerName = reader.GetString("CustomerName"),
        //                        CustomerEmail = reader.GetString("CustomerEmail"),
        //                        TotalAmount = reader.GetDecimal("TotalAmount"),
        //                        Status = (InvoiceStatus)reader.GetInt32("Status"),
        //                        CreatedAt = reader.GetDateTime("CreatedAt"),
        //                        UpdatedAt = reader.GetDateTime("UpdatedAt")
        //                    });
        //                }
        //            }
        //        }
        //    }
        //    return invoices;
        //}

        //public IEnumerable<Invoice> GetInvoices()
        //{
        //    var invoices = new List<Invoice>();
        //    using (var connection = _dbHelper.GetConnection())
        //    {
        //        using (var command = new SqlCommand("SELECT * FROM Invoices", connection))
        //        {
        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    invoices.Add(new Invoice
        //                    {
        //                        Id = reader.GetGuid("Id"),
        //                        CustomerName = reader.GetString("CustomerName"),
        //                        CustomerEmail = reader.GetString("CustomerEmail"),
        //                        TotalAmount = reader.GetDecimal("TotalAmount"),

        //                        // Assuming Status is stored as an integer in the database
        //                        Status = (InvoiceStatus)reader.GetInt32("Status"),

        //                        CreatedAt = reader.GetDateTime("CreatedAt"),
        //                        UpdatedAt = reader.GetDateTime("UpdatedAt")
        //                    });
        //                }
        //            }
        //        }
        //    }
        //    return invoices;
        //}

        public IEnumerable<Invoice> GetInvoices()
        {
            var invoices = new List<Invoice>();


            using (var connection = _dbHelper.GetConnection())
            {
                using (var command = new SqlCommand("SELECT * FROM Invoices", connection))
                {
                    if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            invoices.Add(new Invoice
                            {
                                //Id = reader.GetGuid("Id"),
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),

                                CustomerName = reader.GetString("CustomerName"),
                                CustomerEmail = reader.GetString("CustomerEmail"),
                                TotalAmount = reader.GetDecimal("Amount"),
                                //Status = (InvoiceStatus)reader.GetInt32("Status"),
                                Status = Enum.TryParse<InvoiceStatus>(reader.GetString(reader.GetOrdinal("Status")), out var status)
                 ? status
                 : InvoiceStatus.Pending,  // Default value if invalid
                                CreatedAt = reader.GetDateTime("CreatedAt"),
                                UpdatedAt = reader.GetDateTime("UpdatedAt")
                            });
                        }
                    }
                }
            }

            return invoices;
        }


        public void CreateInvoice(Invoice invoice)
        {
            //using (var connection = _dbHelper.GetConnection())
            //{
            //    using (var command = new SqlCommand("INSERT INTO Invoices ( CustomerName, CustomerEmail, Amount, Status, CreatedAt, UpdatedAt) VALUES (s @CustomerName, @CustomerEmail, @TotalAmount, @Status, @CreatedAt, @UpdatedAt)", connection))
            //    {

            //        command.Parameters.AddWithValue("@Id", invoice.Id);
            //        command.Parameters.AddWithValue("@CustomerName", invoice.CustomerName);
            //        command.Parameters.AddWithValue("@CustomerEmail", invoice.CustomerEmail);
            //        command.Parameters.AddWithValue("@TotalAmount", invoice.TotalAmount);
            //        command.Parameters.AddWithValue("@Status", (int)invoice.Status);
            //        command.Parameters.AddWithValue("@CreatedAt", invoice.CreatedAt);
            //        command.Parameters.AddWithValue("@UpdatedAt", invoice.UpdatedAt);

            //        command.ExecuteNonQuery();
            //    }
            //}

            try
            {
                using (var connection = _dbHelper.GetConnection())
                {
                    if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();

                    using (var command = new SqlCommand("INSERT INTO Invoices (CustomerName, CustomerEmail, Amount, Status, CreatedAt, UpdatedAt) VALUES (@CustomerName, @CustomerEmail, @TotalAmount, @Status, @CreatedAt, @UpdatedAt)", connection))
                    {
                        command.Parameters.AddWithValue("@CustomerName", invoice.CustomerName);
                        command.Parameters.AddWithValue("@CustomerEmail", invoice.CustomerEmail);
                        command.Parameters.AddWithValue("@TotalAmount", invoice.TotalAmount);
                        command.Parameters.AddWithValue("@Status", (int)invoice.Status);
                        command.Parameters.AddWithValue("@CreatedAt", invoice.CreatedAt);
                        command.Parameters.AddWithValue("@UpdatedAt", invoice.UpdatedAt);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log the SQL exception
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                // Optionally rethrow or handle accordingly
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Optionally rethrow or handle accordingly
            }
            finally
            {
                // Additional cleanup if necessary
                Console.WriteLine("Operation completed, resources cleaned up.");
            }

        }

        public void UpdateInvoice(Invoice invoice)
        {
            using (var connection = _dbHelper.GetConnection())
            {
                using (var command = new SqlCommand("UPDATE Invoices SET CustomerName = @CustomerName, CustomerEmail = @CustomerEmail, Amount = @TotalAmount, Status = @Status, UpdatedAt = @UpdatedAt WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", invoice.Id);
                    command.Parameters.AddWithValue("@CustomerName", invoice.CustomerName);
                    command.Parameters.AddWithValue("@CustomerEmail", invoice.CustomerEmail);
                    command.Parameters.AddWithValue("@TotalAmount", invoice.TotalAmount);
                    command.Parameters.AddWithValue("@Status", (int)invoice.Status);
                    command.Parameters.AddWithValue("@UpdatedAt", invoice.UpdatedAt);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteInvoice(int id)
        {
            using (var connection = _dbHelper.GetConnection())
            {
                using (var command = new SqlCommand("DELETE FROM Invoices WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

    }



}

