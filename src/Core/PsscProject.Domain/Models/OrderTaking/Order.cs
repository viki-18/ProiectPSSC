namespace PsscProject.Domain.Models.OrderTaking;

public class Order
{
    public OrderId Id { get; init; }
    public CustomerId CustomerId { get; init; }
    public DateTime CreatedAt { get; init; }
    public List<OrderLine> Lines { get; init; }

    public Money Total => Lines.Aggregate(Money.Zero, (acc, line) => acc + line.Total);

    private Order() { }
    
    private Order(OrderId id, CustomerId customerId, DateTime createdAt, List<OrderLine> lines)
    {
        Id = id;
        CustomerId = customerId;
        CreatedAt = createdAt;
        Lines = lines;
    }

    public static Order Create(CustomerId customerId, List<OrderLine> lines)
    {
        if (lines.Count == 0)
            throw new InvalidOperationException("Order must have at least one line");

        return new Order(
            new OrderId(Guid.NewGuid()),
            customerId,
            DateTime.UtcNow,
            lines
        );
    }
}
