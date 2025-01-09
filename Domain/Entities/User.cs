using System;
using System.ComponentModel.DataAnnotations;

namespace invoice_task.Domain.Entities
{
    public class User
    {

        //public Guid Id { get; set; }             // Unique identifier for the user
        //public string Username { get; set; }    // Username for authentication
        //public string PasswordHash { get; set; } // Hashed password for security
        //public string Role { get; set; }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }



    }
}
