using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Domain.Models.Shipping;

/// <summary>
/// Entitate de domeniu: Shipment - o expediere pentru o comandă
/// </summary>
public class Shipment
{
    public ShipmentId Id { get; init; }
    public OrderId OrderId { get; init; }
    public CustomerId CustomerId { get; init; }
    public ShipmentStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public List<ShipmentLine> Lines { get; init; }

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

    /// <summary>
    /// Factory method - creează o expediere nouă din datele unei comenzi
    /// </summary>
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

    /// <summary>
    /// Marchez expeditia ca fiind în tranzit
    /// </summary>
    public void MarkAsInTransit()
    {
        if (Status != ShipmentStatus.Packed)
            throw new InvalidOperationException("Can only mark as in-transit when status is Packed");
        
        // În domeniu nu pot schimba direct - ar trebui să ridic eveniment
        // Pentru moment, simplificăm
    }
}
