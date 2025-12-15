using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Domain.Interfaces.OrderTaking;

public interface IProductCatalog
{
    Task<ProductDetails?> GetProductAsync(ProductId productId);
    Task<bool> IsProductAvailableAsync(ProductId productId, int quantity);
}

public record ProductDetails(
    ProductId Id,
    string Name,
    Money Price,
    int StockQuantity
);
