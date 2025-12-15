namespace PsscProject.Domain.Models.OrderTaking
{
    // Daca nu ai clasa DomainEvent, sterge ": DomainEvent"
    public record OrderCancelledEvent(Guid OrderId); 
}