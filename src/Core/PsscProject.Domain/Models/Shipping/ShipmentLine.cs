using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Domain.Models.Shipping;

public record ShipmentLine(
    ProductId ProductId,
    string ProductName,
    int Quantity
)
{
    private ShipmentLine() : this(default!, default!, default) { }
    public static ShipmentLine FromOrderLine(OrderLine orderLine)
    {
        return new ShipmentLine(
            orderLine.ProductId,
            orderLine.ProductName,
            orderLine.Quantity
        );
    }
}
