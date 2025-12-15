using PsscProject.Domain.Models.Invoicing;
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Domain.Interfaces.Invoicing;

public interface IInvoicesRepository
{
    Task SaveInvoiceAsync(Invoice invoice);
    Task<Invoice?> GetInvoiceByIdAsync(InvoiceId invoiceId);
    Task<List<Invoice>> GetInvoicesByOrderAsync(OrderId orderId);
    Task<List<Invoice>> GetInvoicesByCustomerAsync(CustomerId customerId);
}
