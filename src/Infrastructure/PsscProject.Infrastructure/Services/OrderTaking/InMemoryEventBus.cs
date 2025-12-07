using PsscProject.Domain.Interfaces.OrderTaking;
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Infrastructure.Services.OrderTaking;

/// <summary>
/// Event bus in-memory (temporar, înainte de message broker real)
/// Suportă publicare de evenimente și handlers
/// </summary>
public class InMemoryEventBus : IEventBus
{
    private readonly List<object> _events = new();
    private readonly Dictionary<Type, List<Delegate>> _handlers = new();

    public Task PublishAsync(object domainEvent)
    {
        _events.Add(domainEvent);
        Console.WriteLine($"[EventBus] Published: {domainEvent.GetType().Name}");

        // Execută toți handlerii pentru acest tip de eveniment
        var eventType = domainEvent.GetType();
        if (_handlers.TryGetValue(eventType, out var handlers))
        {
            foreach (var handler in handlers)
            {
                if (handler is Delegate del)
                {
                    // Apelează handlerul (poate fi async)
                    var task = del.DynamicInvoke(domainEvent) as Task;
                    if (task != null)
                    {
                        task.Wait(); // Așteptă ca handlerul să se termine
                    }
                }
            }
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Înregistrează un handler pentru un tip specific de eveniment
    /// </summary>
    public void Subscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : class
    {
        var eventType = typeof(TEvent);
        if (!_handlers.ContainsKey(eventType))
        {
            _handlers[eventType] = new List<Delegate>();
        }
        _handlers[eventType].Add(handler);
    }

    public List<object> GetPublishedEvents() => new(_events);
}
