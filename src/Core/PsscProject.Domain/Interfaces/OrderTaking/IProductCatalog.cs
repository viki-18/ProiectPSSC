using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Domain.Interfaces.OrderTaking;

/// <summary>
/// Serviciu pentru a citi detaliile produselor din catalog
/// </summary>
public interface IProductCatalog
{
    Task<ProductDetails?> GetProductAsync(ProductId productId);
    Task<bool> IsProductAvailableAsync(ProductId productId, int quantity);
}

/// <summary>
/// Detaliile unui produs din catalog
/// </summary>
public record ProductDetails(
    ProductId Id,
    string Name,
    Money Price,
    int StockQuantity
);
