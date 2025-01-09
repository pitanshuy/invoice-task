using System;
using System.Collections.Generic;
using invoice_task.Domain.Entities;

namespace invoice_task.Interfaces
{
    public interface IInvoiceRepository
    {


        IEnumerable<Invoice> GetInvoices();
        void CreateInvoice(Invoice invoice);
        void UpdateInvoice(Invoice invoice);
        void DeleteInvoice(int id);

    }
}
