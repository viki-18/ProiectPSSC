namespace PsscProject.Domain.Models.OrderTaking;

public record OrderPlacedEvent(
    Guid OrderId,
    Guid CustomerId,
    decimal TotalAmount,
    string Currency,
    int LineCount,
    DateTime Timestamp
);
