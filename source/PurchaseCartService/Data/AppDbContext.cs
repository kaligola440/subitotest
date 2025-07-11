using Microsoft.EntityFrameworkCore;
using PurchaseCartService.Models;

namespace PurchaseCartService.Data;

public class AppDbContext : DbContext
{
    public DbSet<ProductContract> Products => Set<ProductContract>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // Seeding prodotti
        mb.Entity<ProductContract>().HasData(
            // Alimentari (IVA 10%)
            new ProductContract { ProductId = 1, Price = 2.00, VatRate = 0.10 },
            new ProductContract { ProductId = 2, Price = 1.50, VatRate = 0.10 },
            new ProductContract { ProductId = 3, Price = 3.00, VatRate = 0.10 },
            // Libri (IVA 4%)
            new ProductContract { ProductId = 4, Price = 15.90, VatRate = 0.04 },
            new ProductContract { ProductId = 5, Price = 28.00, VatRate = 0.04 },
            // Elettronica (IVA 22%)
            new ProductContract { ProductId = 6, Price = 399.99, VatRate = 0.22 },
            new ProductContract { ProductId = 7, Price = 899.00, VatRate = 0.22 },
            new ProductContract { ProductId = 8, Price = 24.95, VatRate = 0.22 },
            new ProductContract { ProductId = 9, Price = 49.90, VatRate = 0.22 },
            new ProductContract { ProductId = 10, Price = 59.99, VatRate = 0.22 }
        );
    }
}
