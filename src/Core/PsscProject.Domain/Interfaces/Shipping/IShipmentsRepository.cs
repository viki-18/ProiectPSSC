using PsscProject.Domain.Models.Shipping;
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Domain.Interfaces.Shipping;

/// <summary>
/// Repository pentru expedieri (citire/scriere)
/// </summary>
public interface IShipmentsRepository
{
    Task SaveShipmentAsync(Shipment shipment);
    Task<Shipment?> GetShipmentByIdAsync(ShipmentId shipmentId);
    Task<List<Shipment>> GetShipmentsByOrderAsync(OrderId orderId);
    Task<List<Shipment>> GetShipmentsByCustomerAsync(CustomerId customerId);
}
