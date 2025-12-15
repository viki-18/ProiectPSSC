using PsscProject.Domain.Interfaces.OrderTaking;
using PsscProject.Domain.Models;
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Application.Workflows.OrderTaking
{
    public class CancelOrderWorkflow
    {
        private readonly IOrdersRepository _repository;
        private readonly IEventBus _eventBus;

        public CancelOrderWorkflow(IOrdersRepository repository, IEventBus eventBus)
        {
            _repository = repository;
            _eventBus = eventBus;
        }

        public async Task<Result<OrderCancelledEvent>> ExecuteAsync(CancelOrderCommand command)
        {
            var orderId = new OrderId(command.OrderId);
            var order = await _repository.GetOrderByIdAsync(orderId);

            if (order == null)
                return Result<OrderCancelledEvent>.Failure($"Order with ID {command.OrderId} not found.");

            // Logica de anulare
            var result = order.Cancel();
            if (result.IsFailure) return Result<OrderCancelledEvent>.Failure(result.Error);

            await _repository.SaveOrderAsync(order); // Sau UpdateOrderAsync

            var orderCancelledEvent = new OrderCancelledEvent(order.Id.Value);
            await _eventBus.PublishAsync(orderCancelledEvent);

            return Result<OrderCancelledEvent>.Success(orderCancelledEvent);
        }
    }
}