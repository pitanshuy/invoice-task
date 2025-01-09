using System.Collections.Generic;
using System;
using invoice_task.Domain.Entities;

namespace invoice_task.Interfaces
{
    public interface IInvoiceService
    {
        //List<Invoice> GetInvoices();
        //IEnumerable<Invoice> GetInvoices();

        IEnumerable<Invoice> GetInvoices(int page, int pageSize, string customerName = null, string email = null);

        Invoice GetInvoiceById(int id);
        void CreateInvoice(Invoice invoice);
        void UpdateInvoice(Invoice invoice);
        void DeleteInvoice(int id);


    }
}
