using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using invoice_task.Interfaces;
using invoice_task.Domain.Entities;
using invoice_task.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using invoice_task.Service;

//using invoice_task.Service;

namespace invoice_task.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILogger<InvoicesController> _logger;
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceRepository invoiceRepository, IInvoiceService invoiceService, ILogger<InvoicesController> logger)
        {
            _invoiceRepository = invoiceRepository;
            _logger = logger;
            _invoiceService = invoiceService;
            
        }


        [HttpGet]
        public IActionResult GetInvoices(int page = 1, int pageSize = 10, string customerName = null, string email = null)
        {
            try
            {
                _logger.LogInformation("Getting invoices with pagination and filtering...");
                var invoices = _invoiceService.GetInvoices(page, pageSize, customerName, email);
                _logger.LogInformation("Invoices retrieved successfully.");

                return Ok(new
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = invoices.Count(),
                    Data = invoices
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving invoices.");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost("CreateInvoice")]
        public IActionResult CreateInvoice([FromBody] Invoice invoice)
        {
            try
            {
                _logger.LogInformation("Creating a new invoice...");

                //invoice.Id = Guid.NewGuid();
                invoice.Id = 1;
                invoice.CreatedAt = DateTime.UtcNow;
                invoice.UpdatedAt = DateTime.UtcNow;

                _invoiceRepository.CreateInvoice(invoice);

                _logger.LogInformation("Invoice created successfully with ID: {InvoiceId}", invoice.Id);
                return CreatedAtAction(nameof(GetInvoices), new { id = invoice.Id }, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating an invoice.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("UpdateInvoice/{id}")]
        public IActionResult UpdateInvoice(int id, [FromBody] Invoice invoice)
        {
            try
            {
                _logger.LogInformation("Updating invoice with ID: {InvoiceId}", id);

                if (id != invoice.Id)
                {
                    _logger.LogWarning("ID mismatch during invoice update. Provided ID: {Id}, Invoice ID: {InvoiceId}", id, invoice.Id);
                    return BadRequest("ID mismatch.");
                }

                invoice.UpdatedAt = DateTime.UtcNow;
                _invoiceRepository.UpdateInvoice(invoice);

                _logger.LogInformation("Invoice with ID: {InvoiceId} updated successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating invoice with ID: {InvoiceId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteInvoice/{id}")]
        public IActionResult DeleteInvoice(int id)
        {
            try
            {
                _logger.LogInformation("Deleting invoice with ID: {InvoiceId}", id);

                _invoiceRepository.DeleteInvoice(id);

                _logger.LogInformation("Invoice with ID: {InvoiceId} deleted successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting invoice with ID: {InvoiceId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
