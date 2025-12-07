using PsscProject.Domain.Interfaces.OrderTaking;
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Infrastructure.Repositories.OrderTaking;

/// <summary>
/// Repository in-memory pentru comenzi (temporar, înainte de baza de date)
/// </summary>
public class InMemoryOrdersRepository : IOrdersRepository
{
    private readonly List<Order> _orders = new();

    public Task SaveOrderAsync(Order order)
    {
        _orders.Add(order);
        return Task.CompletedTask;
    }

    public Task<Order?> GetOrderByIdAsync(OrderId orderId)
    {
        var order = _orders.FirstOrDefault(o => o.Id == orderId);
        return Task.FromResult(order);
    }

    public Task<List<Order>> GetOrdersByCustomerAsync(CustomerId customerId)
    {
        var orders = _orders.Where(o => o.CustomerId == customerId).ToList();
        return Task.FromResult(orders);
    }
}
