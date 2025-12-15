namespace PsscProject.Domain.Models.Shipping;

public record ShipmentCreatedEvent(
    Guid ShipmentId,
    Guid OrderId,
    Guid CustomerId,
    int LineCount,
    DateTime Timestamp
);
