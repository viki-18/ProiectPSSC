namespace PsscProject.Domain.Models.OrderTaking;

public record PlaceOrderCommand(
    Guid CustomerId,
    List<OrderLineDto> Lines
);

public record OrderLineDto(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal Price
);
