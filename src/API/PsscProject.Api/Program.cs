using PsscProject.Application.EventHandlers;
using PsscProject.Application.Workflows.Invoicing;
using PsscProject.Application.Workflows.OrderTaking;
using PsscProject.Application.Workflows.Shipping;
using PsscProject.Domain.Interfaces.Invoicing;
using PsscProject.Domain.Interfaces.OrderTaking;
using PsscProject.Domain.Interfaces.Shipping;
using PsscProject.Domain.Models.Invoicing;
using PsscProject.Domain.Models.OrderTaking;
using PsscProject.Infrastructure.Repositories.Invoicing;
using PsscProject.Infrastructure.Repositories.OrderTaking;
using PsscProject.Infrastructure.Repositories.Shipping;
using PsscProject.Infrastructure.Services.OrderTaking;

var builder = WebApplication.CreateBuilder(args);

// ========== DEPENDENCY INJECTION SETUP ==========

// Order-Taking services
builder.Services.AddSingleton<IOrdersRepository, InMemoryOrdersRepository>();
builder.Services.AddSingleton<IProductCatalog, InMemoryProductCatalog>();
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();

// Invoicing services
builder.Services.AddSingleton<IInvoicesRepository, InMemoryInvoicesRepository>();

// Shipping services
builder.Services.AddSingleton<IShipmentsRepository, InMemoryShipmentsRepository>();

// Application workflows
builder.Services.AddSingleton<PlaceOrderWorkflow>();
builder.Services.AddSingleton<CreateInvoiceWorkflow>();
builder.Services.AddSingleton<CreateShipmentWorkflow>();

// Event handlers
builder.Services.AddSingleton<OrderPlacedEventHandler>();
builder.Services.AddSingleton<InvoiceCreatedEventHandler>();

// Controllers
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// ========== SETUP EVENT HANDLERS ==========
// Înregistrez event handlers la startup
var eventBus = app.Services.GetRequiredService<IEventBus>();
var orderPlacedEventHandler = app.Services.GetRequiredService<OrderPlacedEventHandler>();
var invoiceCreatedEventHandler = app.Services.GetRequiredService<InvoiceCreatedEventHandler>();

if (eventBus is InMemoryEventBus inMemoryEventBus)
{
    inMemoryEventBus.Subscribe<OrderPlacedEvent>(orderPlacedEventHandler.GetHandler());
    Console.WriteLine("[Startup] Registered OrderPlacedEvent handler -> CreateInvoiceWorkflow");

    inMemoryEventBus.Subscribe<InvoiceCreatedEvent>(invoiceCreatedEventHandler.GetHandler());
    Console.WriteLine("[Startup] Registered InvoiceCreatedEvent handler -> CreateShipmentWorkflow");
}

// ========== HTTP PIPELINE ==========

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection(); // Commented out for development with HTTP
app.MapControllers();

app.Run();

