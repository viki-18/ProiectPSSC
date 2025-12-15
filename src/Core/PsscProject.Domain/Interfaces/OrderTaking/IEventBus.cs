namespace PsscProject.Domain.Interfaces.OrderTaking;

public interface IEventBus
{
    Task PublishAsync(object domainEvent);
}
