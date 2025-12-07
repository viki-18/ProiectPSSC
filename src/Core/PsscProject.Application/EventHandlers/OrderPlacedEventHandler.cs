using PsscProject.Application.Workflows.Invoicing;
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Application.EventHandlers;

/// <summary>
/// Event handler: La primirea evenimentului "OrderPlaced", declanșez CreateInvoiceWorkflow
/// </summary>
public class OrderPlacedEventHandler
{
    private readonly CreateInvoiceWorkflow _createInvoiceWorkflow;

    public OrderPlacedEventHandler(CreateInvoiceWorkflow createInvoiceWorkflow)
    {
        _createInvoiceWorkflow = createInvoiceWorkflow;
    }

    /// <summary>
    /// Obține o metodă care să gestioneze evenimentul OrderPlaced
    /// </summary>
    public Func<OrderPlacedEvent, Task> GetHandler()
    {
        return async (orderPlacedEvent) =>
        {
            Console.WriteLine($"[EventHandler] Received OrderPlaced event for Order: {orderPlacedEvent.OrderId}");
            
            try
            {
                var invoiceCreatedEvent = await _createInvoiceWorkflow.ExecuteAsync(
                    new OrderId(orderPlacedEvent.OrderId)
                );

                Console.WriteLine($"[EventHandler] Invoice created: {invoiceCreatedEvent.InvoiceId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EventHandler] Error creating invoice: {ex.Message}");
                throw;
            }
        };
    }
}
