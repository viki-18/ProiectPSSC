using PsscProject.Domain.Interfaces.Invoicing;
using PsscProject.Domain.Interfaces.OrderTaking;
using PsscProject.Domain.Models.Invoicing;
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Application.Workflows.Invoicing;

/// <summary>
/// Workflow: Create Invoice
/// - Se declanșează atunci când o comandă a fost plasată (OrderPlaced event)
/// - Citește liniile comenzii
/// - Creează o factură
/// - Publică evenimentul "InvoiceCreated"
/// </summary>
public class CreateInvoiceWorkflow
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IInvoicesRepository _invoicesRepository;
    private readonly IEventBus _eventBus;

    public CreateInvoiceWorkflow(
        IOrdersRepository ordersRepository,
        IInvoicesRepository invoicesRepository,
        IEventBus eventBus)
    {
        _ordersRepository = ordersRepository;
        _invoicesRepository = invoicesRepository;
        _eventBus = eventBus;
    }

    /// <summary>
    /// Execută workflow-ul de creare a unei facturi din comanda existentă
    /// </summary>
    public async Task<InvoiceCreatedEvent> ExecuteAsync(OrderId orderId)
    {
        // 1. Citim comanda din repository
        var order = await _ordersRepository.GetOrderByIdAsync(orderId);
        if (order == null)
            throw new InvalidOperationException($"Order {orderId.Value} not found");

        // 2. Convertim liniile comenzii în liniile facturii
        var invoiceLines = order.Lines
            .Select(orderLine => InvoiceLine.FromOrderLine(orderLine))
            .ToList();

        // 3. Creăm factura
        var invoice = Invoice.CreateFromOrder(
            order.Id,
            order.CustomerId,
            invoiceLines
        );

        // 4. Salvăm factura
        await _invoicesRepository.SaveInvoiceAsync(invoice);

        // 5. Creăm și publicăm evenimentul "InvoiceCreated"
        var invoiceCreatedEvent = new InvoiceCreatedEvent(
            invoice.Id.Value,
            invoice.OrderId.Value,
            invoice.CustomerId.Value,
            invoice.Total.Amount,
            invoice.Total.Currency,
            invoice.Lines.Count,
            DateTime.UtcNow
        );

        await _eventBus.PublishAsync(invoiceCreatedEvent);

        return invoiceCreatedEvent;
    }
}
