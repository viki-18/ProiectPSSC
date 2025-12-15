using PsscProject.Domain.Interfaces.OrderTaking;
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Application.Workflows.OrderTaking;

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

    public async Task<OrderPlacedEvent> ExecuteAsync(PlaceOrderCommand command)
    {
        if (command.Lines == null || command.Lines.Count == 0)
            throw new InvalidOperationException("Order must have at least one line");

        var orderLines = new List<OrderLine>();

        foreach (var line in command.Lines)
        {
            var productId = new ProductId(line.ProductId);

            var product = await _productCatalog.GetProductAsync(productId);
            if (product == null)
                throw new InvalidOperationException($"Product {line.ProductId} not found in catalog");

            if (!await _productCatalog.IsProductAvailableAsync(productId, line.Quantity))
                throw new InvalidOperationException(
                    $"Product {line.ProductName} is not available in the requested quantity");

            var orderLine = OrderLine.Create(
                productId,
                product.Name,
                line.Quantity,
                product.Price
            );
            orderLines.Add(orderLine);
        }

        var order = Order.Create(
            new CustomerId(command.CustomerId),
            orderLines
        );

        await _ordersRepository.SaveOrderAsync(order);

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
