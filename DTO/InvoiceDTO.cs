using System;

namespace invoice_task.DTO
{
    public class InvoiceDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }

    }
}
