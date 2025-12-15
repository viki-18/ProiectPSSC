using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Domain.Models.Invoicing;

/// <summary>
/// O linie din factură (corespunde cu o linie de comandă)
/// </summary>
public record InvoiceLine(
    ProductId ProductId,
    string ProductName,
    int Quantity,
    Money UnitPrice
)
{
    private InvoiceLine() : this(default!, default!, default, default!) { }
    public Money Total => UnitPrice * Quantity;

    public static InvoiceLine FromOrderLine(OrderLine orderLine)
    {
        return new InvoiceLine(
            orderLine.ProductId,
            orderLine.ProductName,
            orderLine.Quantity,
            orderLine.Price
        );
    }
}
