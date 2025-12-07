using PsscProject.Domain.Interfaces.Invoicing;
using PsscProject.Domain.Models.Invoicing;
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Infrastructure.Repositories.Invoicing;

/// <summary>
/// Repository in-memory pentru facturi (temporar, înainte de baza de date)
/// </summary>
public class InMemoryInvoicesRepository : IInvoicesRepository
{
    private readonly List<Invoice> _invoices = new();

    public Task SaveInvoiceAsync(Invoice invoice)
    {
        _invoices.Add(invoice);
        return Task.CompletedTask;
    }

    public Task<Invoice?> GetInvoiceByIdAsync(InvoiceId invoiceId)
    {
        var invoice = _invoices.FirstOrDefault(i => i.Id == invoiceId);
        return Task.FromResult(invoice);
    }

    public Task<List<Invoice>> GetInvoicesByOrderAsync(OrderId orderId)
    {
        var invoices = _invoices.Where(i => i.OrderId == orderId).ToList();
        return Task.FromResult(invoices);
    }

    public Task<List<Invoice>> GetInvoicesByCustomerAsync(CustomerId customerId)
    {
        var invoices = _invoices.Where(i => i.CustomerId == customerId).ToList();
        return Task.FromResult(invoices);
    }
}
