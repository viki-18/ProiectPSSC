namespace PsscProject.Domain.Models.OrderTaking;

/// <summary>
/// Comanda (input) pentru workflow-ul "Place Order"
/// </summary>
public record PlaceOrderCommand(
    Guid CustomerId,
    List<OrderLineDto> Lines
);

/// <summary>
/// DTO pentru liniile de comandă (input)
/// </summary>
public record OrderLineDto(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal Price
);
