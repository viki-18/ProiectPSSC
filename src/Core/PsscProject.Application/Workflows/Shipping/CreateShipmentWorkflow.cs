using PsscProject.Domain.Interfaces.OrderTaking;
using PsscProject.Domain.Interfaces.Shipping;
using PsscProject.Domain.Models.OrderTaking;
using PsscProject.Domain.Models.Shipping;

namespace PsscProject.Application.Workflows.Shipping;

/// <summary>
/// Workflow: Create Shipment
/// - Se declanșează atunci când o factură a fost creată (InvoiceCreated event)
/// - Citește liniile comenzii
/// - Creează o expediere
/// - Publică evenimentul "ShipmentCreated"
/// </summary>
public class CreateShipmentWorkflow
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IShipmentsRepository _shipmentsRepository;
    private readonly IEventBus _eventBus;

    public CreateShipmentWorkflow(
        IOrdersRepository ordersRepository,
        IShipmentsRepository shipmentsRepository,
        IEventBus eventBus)
    {
        _ordersRepository = ordersRepository;
        _shipmentsRepository = shipmentsRepository;
        _eventBus = eventBus;
    }

    /// <summary>
    /// Execută workflow-ul de creare a unei expedieri din comanda existentă
    /// </summary>
    public async Task<ShipmentCreatedEvent> ExecuteAsync(OrderId orderId)
    {
        // 1. Citim comanda din repository
        var order = await _ordersRepository.GetOrderByIdAsync(orderId);
        if (order == null)
            throw new InvalidOperationException($"Order {orderId.Value} not found");

        // 2. Convertim liniile comenzii în liniile expedieri
        var shipmentLines = order.Lines
            .Select(orderLine => ShipmentLine.FromOrderLine(orderLine))
            .ToList();

        // 3. Creăm expeditia
        var shipment = Shipment.CreateFromOrder(
            order.Id,
            order.CustomerId,
            shipmentLines
        );

        // 4. Salvăm expeditia
        await _shipmentsRepository.SaveShipmentAsync(shipment);

        // 5. Creăm și publicăm evenimentul "ShipmentCreated"
        var shipmentCreatedEvent = new ShipmentCreatedEvent(
            shipment.Id.Value,
            shipment.OrderId.Value,
            shipment.CustomerId.Value,
            shipment.Lines.Count,
            DateTime.UtcNow
        );

        await _eventBus.PublishAsync(shipmentCreatedEvent);

        return shipmentCreatedEvent;
    }
}
