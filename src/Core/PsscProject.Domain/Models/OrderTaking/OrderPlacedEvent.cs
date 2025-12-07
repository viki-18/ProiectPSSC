namespace PsscProject.Domain.Models.OrderTaking;

/// <summary>
/// Eveniment de domeniu: "Order Placed" - se ridică atunci când o comandă a fost preluată cu succes
/// </summary>
public record OrderPlacedEvent(
    Guid OrderId,
    Guid CustomerId,
    decimal TotalAmount,
    string Currency,
    int LineCount,
    DateTime Timestamp
);
