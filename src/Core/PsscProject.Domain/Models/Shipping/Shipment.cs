using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Domain.Models.Shipping;

public class Shipment
{
    public ShipmentId Id { get; init; }
    public OrderId OrderId { get; init; }
    public CustomerId CustomerId { get; init; }
    public ShipmentStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public List<ShipmentLine> Lines { get; init; }

    private Shipment() { }
    
    private Shipment(
        ShipmentId id,
        OrderId orderId,
        CustomerId customerId,
        DateTime createdAt,
        List<ShipmentLine> lines,
        ShipmentStatus status = ShipmentStatus.Created)
    {
        Id = id;
        OrderId = orderId;
        CustomerId = customerId;
        CreatedAt = createdAt;
        Lines = lines;
        Status = status;
    }

    public static Shipment CreateFromOrder(OrderId orderId, CustomerId customerId, List<ShipmentLine> lines)
    {
        if (lines.Count == 0)
            throw new InvalidOperationException("Shipment must have at least one line");

        return new Shipment(
            new ShipmentId(Guid.NewGuid()),
            orderId,
            customerId,
            DateTime.UtcNow,
            lines,
            ShipmentStatus.Created
        );
    }

    public void MarkAsInTransit()
    {
        if (Status != ShipmentStatus.Packed)
            throw new InvalidOperationException("Can only mark as in-transit when status is Packed");
    }
}
