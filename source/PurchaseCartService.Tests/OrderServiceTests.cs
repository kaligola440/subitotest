using Microsoft.EntityFrameworkCore;
using PurchaseCartService.Data;
using PurchaseCartService.Models.Requests;
using PurchaseCartService.Services;

public class OrderServiceTests
{

    private AppDbContext CreateInMemoryDb()
    {
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        var ctx = new AppDbContext(opts);
        ctx.Database.OpenConnection();
        ctx.Database.EnsureCreated(); // applica OnModelCreating + seeding
        return ctx;
    }

    [Fact]
    public void CreateOrder_CalculatesTotalsCorrectly()
    {
        using var db = CreateInMemoryDb();
        var svc = new OrderService(db);

        var req = new OrderRequest
        {
            Items = new()
            {
                new OrderItem { ProductId = 1, Quantity = 2 }, // 2 × 2,00 = 4,00
                new OrderItem { ProductId = 4, Quantity = 1 }  // 1 × 15,90 = 15,90
            }
        };

        var res = svc.CreateOrder(req);

        Assert.Equal(19.90, res.OrderPrice, 2);
        Assert.Equal((4 * 0.10) + (15.90 * 0.04), res.OrderVat, 2);
        Assert.Equal(2, res.Items.Count);
    }
}

