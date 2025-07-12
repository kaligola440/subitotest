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
    [Fact]
    public void CreateOrder_SkipsUnknownProducts()
    {
        using var db = CreateInMemoryDb();
        var service = new OrderService(db);

        var request = new OrderRequest
        {
            Items = new List<OrderItem>
            {
                new() { ProductId = 999, Quantity = 1 }, // non esiste
                new() { ProductId = 2, Quantity = 4 }    // 4 * 1.50 = 6.00, IVA 0.60
            }
        };

        var result = service.CreateOrder(request);

        Assert.Single(result.Items);
        Assert.Equal(6.00, result.OrderPrice, 2);
        Assert.Equal(0.60, result.OrderVat, 2);
    }

    [Fact]
    public void CreateOrder_HandlesZeroQuantity()
    {
        using var db = CreateInMemoryDb();
        var service = new OrderService(db);

        var request = new OrderRequest
        {
            Items = new List<OrderItem>
        {
            new() { ProductId = 1, Quantity = 0 }, // deve comparire con price=0, vat=0
            new() { ProductId = 2, Quantity = 3 }  // 3 × 1.50 = 4.50, IVA 0.45
        }
        };

        var result = service.CreateOrder(request);


        Assert.Equal(2, result.Items.Count);

        var zeroItem = result.Items.Single(i => i.ProductId == 1);
        Assert.Equal(0, zeroItem.Price, 2);
        Assert.Equal(0, zeroItem.Vat, 2);

        Assert.Equal(4.50, result.OrderPrice, 2);
        Assert.Equal(0.45, result.OrderVat, 2);
    }
}

