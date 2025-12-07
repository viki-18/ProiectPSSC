using PsscProject.Domain.Interfaces.OrderTaking;
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Application.Workflows.OrderTaking;

/// <summary>
/// Workflow: Place Order
/// - Primește o comandă de la client
/// - Validează produsele și stocul
/// - Salvează comanda în repository
/// - Publică evenimentul "OrderPlaced"
/// </summary>
public class PlaceOrderWorkflow
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IProductCatalog _productCatalog;
    private readonly IEventBus _eventBus;

    public PlaceOrderWorkflow(
        IOrdersRepository ordersRepository,
        IProductCatalog productCatalog,
        IEventBus eventBus)
    {
        _ordersRepository = ordersRepository;
        _productCatalog = productCatalog;
        _eventBus = eventBus;
    }

    /// <summary>
    /// Execută workflow-ul de preluare a unei comenzi
    /// </summary>
    public async Task<OrderPlacedEvent> ExecuteAsync(PlaceOrderCommand command)
    {
        // 1. Validare: Verificăm că avem date valide
        if (command.Lines == null || command.Lines.Count == 0)
            throw new InvalidOperationException("Order must have at least one line");

        // 2. Validare: Verificăm fiecare linie și construim OrderLines
        var orderLines = new List<OrderLine>();

        foreach (var line in command.Lines)
        {
            var productId = new ProductId(line.ProductId);

            // Căutăm produsul în catalog
            var product = await _productCatalog.GetProductAsync(productId);
            if (product == null)
                throw new InvalidOperationException($"Product {line.ProductId} not found in catalog");

            // Verificăm stocul
            if (!await _productCatalog.IsProductAvailableAsync(productId, line.Quantity))
                throw new InvalidOperationException(
                    $"Product {line.ProductName} is not available in the requested quantity");

            // Creăm OrderLine cu datele din catalog
            var orderLine = OrderLine.Create(
                productId,
                product.Name,
                line.Quantity,
                product.Price
            );
            orderLines.Add(orderLine);
        }

        // 3. Creăm entitatea Order
        var order = Order.Create(
            new CustomerId(command.CustomerId),
            orderLines
        );

        // 4. Salvăm comanda în repository
        await _ordersRepository.SaveOrderAsync(order);

        // 5. Creăm și publicăm evenimentul "OrderPlaced"
        var orderPlacedEvent = new OrderPlacedEvent(
            order.Id.Value,
            order.CustomerId.Value,
            order.Total.Amount,
            order.Total.Currency,
            order.Lines.Count,
            DateTime.UtcNow
        );

        await _eventBus.PublishAsync(orderPlacedEvent);

        return orderPlacedEvent;
    }
}
