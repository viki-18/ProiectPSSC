namespace PsscProject.Domain.Models.OrderTaking;

// Un tip "strong" pentru a nu confunda un Guid oarecare cu un ID de comandă.
// Folosim 'record' pentru imutabilitate.
public record OrderId(Guid Value);
