using Microsoft.EntityFrameworkCore;
using PsscProject.Domain.Models.OrderTaking;
using PsscProject.Domain.Models.Invoicing;
using PsscProject.Domain.Models.Shipping;

namespace PsscProject.Infrastructure.Persistence
{
    public class PsscDbContext : DbContext
    {
        public PsscDbContext(DbContextOptions<PsscDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Shipment> Shipments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id)
                    .HasConversion(id => id.Value, value => new OrderId(value));

                entity.Property(e => e.CustomerId)
                    .HasConversion(id => id.Value, value => new CustomerId(value));

                entity.OwnsMany(e => e.Lines, line =>
                {
                    line.WithOwner().HasForeignKey("OrderId");
                    line.HasKey("OrderId", "ProductId");

                    line.Property(l => l.ProductId)
                        .HasConversion(id => id.Value, value => new ProductId(value));

                    line.OwnsOne(l => l.Price, price =>
                    {
                        price.Property(p => p.Amount).HasColumnName("PriceAmount").HasColumnType("decimal(18,2)");
                        price.Property(p => p.Currency).HasColumnName("PriceCurrency").HasMaxLength(3);
                    });
                });
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasConversion(id => id.Value, value => new InvoiceId(value));
                entity.Property(e => e.OrderId).HasConversion(id => id.Value, value => new OrderId(value));
                entity.Property(e => e.CustomerId).HasConversion(id => id.Value, value => new CustomerId(value));
                
                entity.Property(e => e.Status).HasConversion<string>();

                entity.OwnsMany(e => e.Lines, line =>
                {
                    line.WithOwner().HasForeignKey("InvoiceId");
                    line.Property(l => l.ProductId).HasConversion(id => id.Value, value => new ProductId(value));
                    
                    line.OwnsOne(l => l.UnitPrice, price =>
                    {
                        price.Property(p => p.Amount).HasColumnName("UnitPriceAmount").HasColumnType("decimal(18,2)");
                        price.Property(p => p.Currency).HasColumnName("UnitPriceCurrency").HasMaxLength(3);
                    });
                });
            });

            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasConversion(id => id.Value, value => new ShipmentId(value));
                entity.Property(e => e.OrderId).HasConversion(id => id.Value, value => new OrderId(value));
                entity.Property(e => e.CustomerId).HasConversion(id => id.Value, value => new CustomerId(value));
                
                entity.Property(e => e.Status).HasConversion<string>();

                entity.OwnsMany(e => e.Lines, line =>
                {
                    line.WithOwner().HasForeignKey("ShipmentId");
                    line.Property(l => l.ProductId).HasConversion(id => id.Value, value => new ProductId(value));
                });
            });
        }
    }
}