namespace PsscProject.Domain.Models.Invoicing;

/// <summary>
/// Eveniment de domeniu: "Invoice Created" - se ridică când o factură a fost creată
/// </summary>
public record InvoiceCreatedEvent(
    Guid InvoiceId,
    Guid OrderId,
    Guid CustomerId,
    decimal TotalAmount,
    string Currency,
    int LineCount,
    DateTime Timestamp
);
