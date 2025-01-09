using System;

namespace invoice_task.Domain.Entities
{
    public class InvoiceItem
    {

        public Guid Id { get; set; }                // Unique identifier for the item
        public Guid InvoiceId { get; set; }         // Foreign key to the parent invoice
        public string Description { get; set; }     // Description of the item
        public decimal Amount { get; set; }         // Price of the item
    }
}
