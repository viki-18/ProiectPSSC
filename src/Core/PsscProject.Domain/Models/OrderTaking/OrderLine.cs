namespace PsscProject.Domain.Models.OrderTaking;

/// <summary>
/// O linie dintr-o comandă (produs + cantitate + preț)
/// </summary>
public record OrderLine(
    ProductId ProductId,
    string ProductName,
    int Quantity,
    Money Price
)
{
    public Money Total => Price * Quantity;

    public static OrderLine Create(ProductId productId, string productName, int quantity, Money price)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));
        if (price.Amount <= 0)
            throw new ArgumentException("Price must be greater than 0", nameof(price));

        return new OrderLine(productId, productName, quantity, price);
    }
}
