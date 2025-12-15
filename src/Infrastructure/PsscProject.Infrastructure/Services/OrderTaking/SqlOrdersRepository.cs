using Microsoft.EntityFrameworkCore;
using PsscProject.Domain.Interfaces.OrderTaking;
using PsscProject.Domain.Models.OrderTaking;
using PsscProject.Infrastructure.Persistence;

namespace PsscProject.Infrastructure.Repositories.OrderTaking
{
    public class SqlOrdersRepository : IOrdersRepository
    {
        private readonly PsscDbContext _dbContext;

        public SqlOrdersRepository(PsscDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order?> GetOrderByIdAsync(OrderId orderId)
        {
            return await _dbContext.Orders
                .Include(o => o.Lines)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<List<Order>> GetOrdersByCustomerAsync(CustomerId customerId)
        {
            return await _dbContext.Orders
                .Include(o => o.Lines)
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task SaveOrderAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }
    }
}