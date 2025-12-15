using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Domain.Interfaces.OrderTaking;

public interface IOrdersRepository
{
    Task SaveOrderAsync(Order order);
    Task<Order?> GetOrderByIdAsync(OrderId orderId);
    Task<List<Order>> GetOrdersByCustomerAsync(CustomerId customerId);
}
