using PsscProject.Domain.Interfaces.Shipping;
using PsscProject.Domain.Models.Shipping;
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Infrastructure.Repositories.Shipping;

public class InMemoryShipmentsRepository : IShipmentsRepository
{
    private readonly List<Shipment> _shipments = new();

    public Task SaveShipmentAsync(Shipment shipment)
    {
        _shipments.Add(shipment);
        return Task.CompletedTask;
    }

    public Task<Shipment?> GetShipmentByIdAsync(ShipmentId shipmentId)
    {
        var shipment = _shipments.FirstOrDefault(s => s.Id == shipmentId);
        return Task.FromResult(shipment);
    }

    public Task<List<Shipment>> GetShipmentsByOrderAsync(OrderId orderId)
    {
        var shipments = _shipments.Where(s => s.OrderId == orderId).ToList();
        return Task.FromResult(shipments);
    }

    public Task<List<Shipment>> GetShipmentsByCustomerAsync(CustomerId customerId)
    {
        var shipments = _shipments.Where(s => s.CustomerId == customerId).ToList();
        return Task.FromResult(shipments);
    }
}
