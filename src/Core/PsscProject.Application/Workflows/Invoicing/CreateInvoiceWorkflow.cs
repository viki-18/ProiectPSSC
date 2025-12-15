using PsscProject.Domain.Interfaces.Invoicing;
using PsscProject.Domain.Interfaces.OrderTaking;
using PsscProject.Domain.Models.Invoicing;
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Application.Workflows.Invoicing;

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

    public async Task<InvoiceCreatedEvent> ExecuteAsync(OrderId orderId)
    {
        var order = await _ordersRepository.GetOrderByIdAsync(orderId);
        if (order == null)
            throw new InvalidOperationException($"Order {orderId.Value} not found");

        var invoiceLines = order.Lines
            .Select(orderLine => InvoiceLine.FromOrderLine(orderLine))
            .ToList();

        var invoice = Invoice.CreateFromOrder(
            order.Id,
            order.CustomerId,
            invoiceLines
        );

        await _invoicesRepository.SaveInvoiceAsync(invoice);

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
