using System;
using System.Collections.Generic;

namespace invoice_task.Domain.Entities
{
    public class Invoice
    {
        public int Id { get; set; }                // Unique identifier for the invoice
        public string CustomerName { get; set; }    // Name of the customer
        public string CustomerEmail { get; set; }   // Email address of the customer
        public decimal TotalAmount { get; set; }    // Total amount of the invoice
        public InvoiceStatus Status { get; set; }   // Status of the invoice (enum)
        public DateTime CreatedAt { get; set; }     // Timestamp when the invoice was created
        public DateTime UpdatedAt { get; set; }     // Timestamp when the invoice was last updated
        public List<InvoiceItem> Items { get; set; } // List of items in the invoice



        public class InvoiceItem
        {
            public int Id { get; set; }
            public int InvoiceId { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
        }

        public enum InvoiceStatus
        {
            Pending = 0,
            Paid = 1,
            Overdue = 2,
            Canceled = 3
        }




    }
}
