using Microsoft.AspNetCore.Mvc;
using PsscProject.Application.Workflows.Invoicing;
using PsscProject.Domain.Interfaces.Invoicing;
using PsscProject.Domain.Models.Invoicing;
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoicingController : ControllerBase
{
    private readonly CreateInvoiceWorkflow _createInvoiceWorkflow;
    private readonly IInvoicesRepository _invoicesRepository;

    public InvoicingController(
        CreateInvoiceWorkflow createInvoiceWorkflow,
        IInvoicesRepository invoicesRepository)
    {
        _createInvoiceWorkflow = createInvoiceWorkflow;
        _invoicesRepository = invoicesRepository;
    }

    /// <summary>
    /// POST /api/invoicing/create-invoice
    /// Creare factură pentru o comandă existentă
    /// </summary>
    [HttpPost("create-invoice")]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceRequest request)
    {
        try
        {
            var result = await _createInvoiceWorkflow.ExecuteAsync(
                new OrderId(request.OrderId)
            );

            return Ok(new
            {
                success = true,
                invoiceId = result.InvoiceId,
                orderId = result.OrderId,
                customerId = result.CustomerId,
                totalAmount = result.TotalAmount,
                currency = result.Currency,
                lineCount = result.LineCount,
                timestamp = result.Timestamp
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                success = false,
                error = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                error = "Internal server error: " + ex.Message
            });
        }
    }

    /// <summary>
    /// GET /api/invoicing/{invoiceId}
    /// Preia detaliile unei facturi
    /// </summary>
    [HttpGet("{invoiceId}")]
    public async Task<IActionResult> GetInvoice(Guid invoiceId)
    {
        try
        {
            var invoice = await _invoicesRepository.GetInvoiceByIdAsync(
                new InvoiceId(invoiceId)
            );

            if (invoice == null)
                return NotFound(new { error = "Invoice not found" });

            return Ok(new
            {
                invoiceId = invoice.Id.Value,
                orderId = invoice.OrderId.Value,
                customerId = invoice.CustomerId.Value,
                status = invoice.Status.ToString(),
                issuedAt = invoice.IssuedAt,
                totalAmount = invoice.Total.Amount,
                currency = invoice.Total.Currency,
                lines = invoice.Lines.Select(l => new
                {
                    productId = l.ProductId.Value,
                    productName = l.ProductName,
                    quantity = l.Quantity,
                    unitPrice = l.UnitPrice.Amount,
                    total = l.Total.Amount
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                error = "Internal server error: " + ex.Message
            });
        }
    }
}

public record CreateInvoiceRequest(Guid OrderId);
