using PsscProject.Domain.Interfaces.OrderTaking;
using PsscProject.Domain.Models; // Pt Result
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Application.Workflows.OrderTaking
{
    public class PlaceOrderWorkflow
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IProductCatalog _productCatalog;
        private readonly IEventBus _eventBus;

        public PlaceOrderWorkflow(IOrdersRepository ordersRepository, IProductCatalog productCatalog, IEventBus eventBus)
        {
            _ordersRepository = ordersRepository;
            _productCatalog = productCatalog;
            _eventBus = eventBus;
        }

        // Am schimbat tipul returnat in Result<OrderPlacedEvent>
        public async Task<Result<OrderPlacedEvent>> ExecuteAsync(PlaceOrderCommand command)
        {
            if (command.Lines == null || command.Lines.Count == 0)
                return Result<OrderPlacedEvent>.Failure("Order must have at least one line");

            var orderLines = new List<OrderLine>();
            foreach (var line in command.Lines)
            {
                var productId = new ProductId(line.ProductId);
                var product = await _productCatalog.GetProductAsync(productId);

                if (product == null)
                    return Result<OrderPlacedEvent>.Failure($"Product {line.ProductId} not found");

                if (!await _productCatalog.IsProductAvailableAsync(productId, line.Quantity))
                    return Result<OrderPlacedEvent>.Failure($"Insufficient stock for {line.ProductName}");

                // Aici ar trebui sa folosim Factory-ul care returneaza Result, dar simplificam pentru moment
                // Presupunem ca OrderLine.Create e ok sau il invelim in try-catch daca arunca exceptii
                try 
                {
                    var orderLine = OrderLine.Create(productId, product.Name, line.Quantity, product.Price);
                    orderLines.Add(orderLine);
                }
                catch(Exception ex)
                {
                    return Result<OrderPlacedEvent>.Failure(ex.Message);
                }
            }

            var orderResult = Order.Create(new CustomerId(command.CustomerId), orderLines);
            if (orderResult.IsFailure)
                return Result<OrderPlacedEvent>.Failure(orderResult.Error);

            var order = orderResult.Value;

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

            return Result<OrderPlacedEvent>.Success(orderPlacedEvent);
        }
    }
}