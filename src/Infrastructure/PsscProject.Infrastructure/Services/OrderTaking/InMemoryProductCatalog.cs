using PsscProject.Domain.Interfaces.OrderTaking;
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Infrastructure.Services.OrderTaking;

public class InMemoryProductCatalog : IProductCatalog
{
    private readonly List<ProductDetails> _products = new()
    {
        new ProductDetails(
            new ProductId(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            "Laptop",
            new Money(1000m, "USD"),
            10
        ),
        new ProductDetails(
            new ProductId(Guid.Parse("22222222-2222-2222-2222-222222222222")),
            "Mouse",
            new Money(25m, "USD"),
            100
        ),
        new ProductDetails(
            new ProductId(Guid.Parse("33333333-3333-3333-3333-333333333333")),
            "Keyboard",
            new Money(75m, "USD"),
            50
        ),
    };

    public Task<ProductDetails?> GetProductAsync(ProductId productId)
    {
        var product = _products.FirstOrDefault(p => p.Id == productId);
        return Task.FromResult(product);
    }

    public Task<bool> IsProductAvailableAsync(ProductId productId, int quantity)
    {
        var product = _products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
            return Task.FromResult(false);

        return Task.FromResult(product.StockQuantity >= quantity);
    }
}
