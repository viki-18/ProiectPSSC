using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Domain.Interfaces.OrderTaking;

/// <summary>
/// Repository pentru comenzi (citire/scriere)
/// </summary>
public interface IOrdersRepository
{
    Task SaveOrderAsync(Order order);
    Task<Order?> GetOrderByIdAsync(OrderId orderId);
    Task<List<Order>> GetOrdersByCustomerAsync(CustomerId customerId);
}
