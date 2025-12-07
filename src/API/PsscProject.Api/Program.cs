using PsscProject.Application.Workflows.OrderTaking;
using PsscProject.Domain.Interfaces.OrderTaking;
using PsscProject.Infrastructure.Repositories.OrderTaking;
using PsscProject.Infrastructure.Services.OrderTaking;

var builder = WebApplication.CreateBuilder(args);

// ========== DEPENDENCY INJECTION SETUP ==========

// Infrastructure services (in-memory implementations)
builder.Services.AddSingleton<IOrdersRepository, InMemoryOrdersRepository>();
builder.Services.AddSingleton<IProductCatalog, InMemoryProductCatalog>();
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();

// Application workflows
builder.Services.AddScoped<PlaceOrderWorkflow>();

// Controllers
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// ========== HTTP PIPELINE ==========

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

