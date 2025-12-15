using PsscProject.Domain.Interfaces.OrderTaking;
using PsscProject.Domain.Interfaces.Shipping;
using PsscProject.Domain.Models.OrderTaking;
using PsscProject.Domain.Models.Shipping;

namespace PsscProject.Application.Workflows.Shipping;

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

    public async Task<ShipmentCreatedEvent> ExecuteAsync(OrderId orderId)
    {
        var order = await _ordersRepository.GetOrderByIdAsync(orderId);
        if (order == null)
            throw new InvalidOperationException($"Order {orderId.Value} not found");

        var shipmentLines = order.Lines
            .Select(orderLine => ShipmentLine.FromOrderLine(orderLine))
            .ToList();

        var shipment = Shipment.CreateFromOrder(
            order.Id,
            order.CustomerId,
            shipmentLines
        );

        await _shipmentsRepository.SaveShipmentAsync(shipment);

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
