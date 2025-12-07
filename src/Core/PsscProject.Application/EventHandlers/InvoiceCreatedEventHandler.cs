using PsscProject.Application.Workflows.Shipping;
using PsscProject.Domain.Models.Invoicing;
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Application.EventHandlers;

/// <summary>
/// Event handler: La primirea evenimentului "InvoiceCreated", declanșez CreateShipmentWorkflow
/// </summary>
public class InvoiceCreatedEventHandler
{
    private readonly CreateShipmentWorkflow _createShipmentWorkflow;

    public InvoiceCreatedEventHandler(CreateShipmentWorkflow createShipmentWorkflow)
    {
        _createShipmentWorkflow = createShipmentWorkflow;
    }

    /// <summary>
    /// Obține o metodă care să gestioneze evenimentul InvoiceCreated
    /// </summary>
    public Func<InvoiceCreatedEvent, Task> GetHandler()
    {
        return async (invoiceCreatedEvent) =>
        {
            Console.WriteLine($"[EventHandler] Received InvoiceCreated event for Invoice: {invoiceCreatedEvent.InvoiceId}");

            try
            {
                var shipmentCreatedEvent = await _createShipmentWorkflow.ExecuteAsync(
                    new OrderId(invoiceCreatedEvent.OrderId)
                );

                Console.WriteLine($"[EventHandler] Shipment created: {shipmentCreatedEvent.ShipmentId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EventHandler] Error creating shipment: {ex.Message}");
                throw;
            }
        };
    }
}
