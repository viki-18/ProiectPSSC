using PsscProject.Domain.Models; // Necesar pentru Result

namespace PsscProject.Domain.Models.OrderTaking
{
    public class Order
    {
        public OrderId Id { get; private set; }
        public CustomerId CustomerId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public List<OrderLine> Lines { get; private set; }
        
        // 1. NOU: Proprietatea de stare
        public OrderStatus Status { get; private set; }

        public Money Total => Lines.Aggregate(Money.Zero, (acc, line) => acc + line.Total);

        // Constructor privat pentru EF Core (l-ai adaugat anterior)
        private Order() { }

        // Constructorul principal (privat, apelat doar de Factory)
        private Order(OrderId id, CustomerId customerId, DateTime createdAt, List<OrderLine> lines)
        {
            Id = id;
            CustomerId = customerId;
            CreatedAt = createdAt;
            Lines = lines;
            Status = OrderStatus.Placed; // 2. NOU: Setăm starea inițială
        }

        // Factory method pentru crearea comenzii
        // Am schimbat return type in Result<Order> pentru a fi consistent cu proiectul
        public static Result<Order> Create(CustomerId customerId, List<OrderLine> lines)
        {
            if (lines.Count == 0)
                return Result<Order>.Failure("Order must have at least one line");

            var order = new Order(
                new OrderId(Guid.NewGuid()),
                customerId,
                DateTime.UtcNow,
                lines
            );

            return Result<Order>.Success(order);
        }

        // 3. NOU: Metoda de Anulare (Logica de business)
        public Result<bool> Cancel()
        {
            // Regula 1: Nu poți anula o comandă deja expediată
            if (Status == OrderStatus.Shipped)
            {
                return Result<bool>.Failure("Cannot cancel an order that has already been shipped.");
            }
            
            // Regula 2: Nu poți anula o comandă deja anulată
            if (Status == OrderStatus.Cancelled)
            {
                return Result<bool>.Failure("Order is already cancelled.");
            }

            // Schimbare de stare
            Status = OrderStatus.Cancelled;
            
            return Result<bool>.Success(true);
        }
    }
}