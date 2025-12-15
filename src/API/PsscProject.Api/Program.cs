using Microsoft.EntityFrameworkCore;
using PsscProject.Application.EventHandlers;
using PsscProject.Application.Workflows.Invoicing;
using PsscProject.Application.Workflows.OrderTaking;
using PsscProject.Application.Workflows.Shipping;
using PsscProject.Domain.Interfaces.Invoicing;
using PsscProject.Domain.Interfaces.OrderTaking;
using PsscProject.Domain.Interfaces.Shipping;
using PsscProject.Domain.Models.Invoicing;
using PsscProject.Domain.Models.OrderTaking;
using PsscProject.Infrastructure.Persistence;
using PsscProject.Infrastructure.Repositories.Invoicing;
using PsscProject.Infrastructure.Repositories.OrderTaking;
using PsscProject.Infrastructure.Repositories.Shipping;
using PsscProject.Infrastructure.Services.OrderTaking;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PsscDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IOrdersRepository, SqlOrdersRepository>();
builder.Services.AddSingleton<IProductCatalog, InMemoryProductCatalog>();
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();

builder.Services.AddSingleton<IInvoicesRepository, InMemoryInvoicesRepository>();

builder.Services.AddSingleton<IShipmentsRepository, InMemoryShipmentsRepository>();

builder.Services.AddScoped<PlaceOrderWorkflow>();
builder.Services.AddScoped<CreateInvoiceWorkflow>();
builder.Services.AddScoped<CreateShipmentWorkflow>();
builder.Services.AddScoped<CancelOrderWorkflow>(); 

builder.Services.AddScoped<OrderPlacedEventHandler>();
builder.Services.AddScoped<InvoiceCreatedEventHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();