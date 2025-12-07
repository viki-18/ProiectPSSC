using PsscProject.Domain.Interfaces.OrderTaking;

namespace PsscProject.Infrastructure.Services.OrderTaking;

/// <summary>
/// Event bus in-memory (temporar, înainte de message broker real)
/// </summary>
public class InMemoryEventBus : IEventBus
{
    private readonly List<object> _events = new();

    public Task PublishAsync(object domainEvent)
    {
        _events.Add(domainEvent);
        Console.WriteLine($"[EventBus] Published: {domainEvent.GetType().Name}");
        return Task.CompletedTask;
    }

    public List<object> GetPublishedEvents() => new(_events);
}
