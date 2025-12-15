namespace PsscProject.Domain.Models.Invoicing;

public record InvoiceCreatedEvent(
    Guid InvoiceId,
    Guid OrderId,
    Guid CustomerId,
    decimal TotalAmount,
    string Currency,
    int LineCount,
    DateTime Timestamp
);
