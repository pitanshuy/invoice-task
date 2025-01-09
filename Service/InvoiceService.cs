using System;
using System.Collections.Generic;
using invoice_task.Interfaces;
using invoice_task.Domain.Entities;
using System.Linq;

namespace invoice_task.Service
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public IEnumerable<Invoice> GetInvoices(int page, int pageSize, string customerName = null, string email = null)
        {
            if (page <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Page and PageSize must be greater than zero.");
            }

            var invoices = _invoiceRepository.GetInvoices();

            //// Apply filtering
            //if (!string.IsNullOrEmpty(customerName))
            //{
            //    invoices = invoices.Where(i => i.CustomerName.Contains(customerName, StringComparison.OrdinalIgnoreCase));
            //}

            //if (!string.IsNullOrEmpty(email))
            //{
            //    invoices = invoices.Where(i => i.CustomerEmail.Contains(email, StringComparison.OrdinalIgnoreCase));
            //}

            // Apply pagination
            return invoices
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }


        //public IEnumerable<Invoice> GetInvoices(int page, int pageSize, string customerName = null, string email = null)
        //{
        //    var invoices = _invoiceRepository.GetInvoices();

        //    // Apply filtering
        //    if (!string.IsNullOrEmpty(customerName))
        //    {
        //        invoices = invoices.Where(i => i.CustomerName.Contains(customerName, StringComparison.OrdinalIgnoreCase));
        //    }

        //    if (!string.IsNullOrEmpty(email))
        //    {
        //        invoices = invoices.Where(i => i.CustomerEmail.Contains(email, StringComparison.OrdinalIgnoreCase));
        //    }

        //    // Apply pagination
        //    return invoices.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //}

        public Invoice GetInvoiceById(int id)
        {
            // Call the repository to fetch the invoice
            var invoice = _invoiceRepository.GetInvoices().FirstOrDefault(i => i.Id == id);
            if (invoice == null)
            {
                throw new Exception($"Invoice with ID {id} not found."); // Or return null based on your preference
            }
            return invoice;
        }

        public void CreateInvoice(Invoice invoice)
        {
            invoice.Id = 1;
            invoice.CreatedAt = DateTime.UtcNow;
            invoice.UpdatedAt = DateTime.UtcNow;

            _invoiceRepository.CreateInvoice(invoice);
        }

        public void UpdateInvoice(Invoice invoice)
        {
            invoice.UpdatedAt = DateTime.UtcNow;
            _invoiceRepository.UpdateInvoice(invoice);
        }

        public void DeleteInvoice(int id)
        {
            _invoiceRepository.DeleteInvoice(id);
        }
    }
}
