using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Domain.Models.Invoicing;

/// <summary>
/// Entitate de domeniu: Invoice - o factură pentru o comandă
/// </summary>
public class Invoice
{
    public InvoiceId Id { get; init; }
    public OrderId OrderId { get; init; }
    public CustomerId CustomerId { get; init; }
    public InvoiceStatus Status { get; init; }
    public DateTime IssuedAt { get; init; }
    public List<InvoiceLine> Lines { get; init; }

    public Money Total => Lines.Aggregate(Money.Zero, (acc, line) => acc + line.Total);

    private Invoice() { }
    
    private Invoice(
        InvoiceId id,
        OrderId orderId,
        CustomerId customerId,
        DateTime issuedAt,
        List<InvoiceLine> lines,
        InvoiceStatus status = InvoiceStatus.Issued)
    {
        Id = id;
        OrderId = orderId;
        CustomerId = customerId;
        IssuedAt = issuedAt;
        Lines = lines;
        Status = status;
    }

    /// <summary>
    /// Factory method - creează o factură nouă din datele unei comenzi
    /// </summary>
    public static Invoice CreateFromOrder(OrderId orderId, CustomerId customerId, List<InvoiceLine> lines)
    {
        if (lines.Count == 0)
            throw new InvalidOperationException("Invoice must have at least one line");

        return new Invoice(
            new InvoiceId(Guid.NewGuid()),
            orderId,
            customerId,
            DateTime.UtcNow,
            lines,
            InvoiceStatus.Issued
        );
    }
}
