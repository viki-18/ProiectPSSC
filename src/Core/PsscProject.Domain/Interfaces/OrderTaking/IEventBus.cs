namespace PsscProject.Domain.Interfaces.OrderTaking;

/// <summary>
/// Event bus: pentru a publica evenimentele de domeniu (OrderPlaced, etc.)
/// </summary>
public interface IEventBus
{
    Task PublishAsync(object domainEvent);
}
