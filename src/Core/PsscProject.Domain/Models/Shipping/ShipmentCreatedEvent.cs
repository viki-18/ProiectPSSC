namespace PsscProject.Domain.Models.Shipping;

/// <summary>
/// Eveniment de domeniu: "Shipment Created" - se ridică când o expediere a fost creată
/// </summary>
public record ShipmentCreatedEvent(
    Guid ShipmentId,
    Guid OrderId,
    Guid CustomerId,
    int LineCount,
    DateTime Timestamp
);
