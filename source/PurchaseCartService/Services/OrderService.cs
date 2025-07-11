using PurchaseCartService.Models;
using PurchaseCartService.Data;
using PurchaseCartService.Models.Requests;
using PurchaseCartService.Models.Responses;

namespace PurchaseCartService.Services;

public class OrderService
{
    private readonly AppDbContext _db;
    public OrderService(AppDbContext db) => _db = db;

    public OrderResponse CreateOrder(OrderRequest request)
    {
        var response = new OrderResponse
        {
            OrderId = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };

        foreach (var item in request.Items)
        {
            var product = _db.Products.Find(item.ProductId);
            if (product is null) continue;

            var subtotal = product.Price * item.Quantity;
            var vat = subtotal * product.VatRate;

            response.Items.Add(new OrderItemResponse
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = Math.Round(subtotal, 2),
                Vat = Math.Round(vat, 2)
            });

            response.OrderPrice += subtotal;
            response.OrderVat += vat;
        }

        response.OrderPrice = Math.Round(response.OrderPrice, 2);
        response.OrderVat = Math.Round(response.OrderVat, 2);
        return response;
    }
}

/*
using PurchaseCartService.Models;
using PurchaseCartService.Models.Requests;
using PurchaseCartService.Models.Responses;

namespace PurchaseCartService.Services;

public class OrderService
{
    // Catalogo prodotti simulato in memoria
    private readonly Dictionary<int, ProductContract> _catalog = new()
    {
        // Alimentari (IVA 10%)
        { 1,  new ProductContract { ProductId = 1,  Price = 2.00, VatRate = 0.10 } },  // Pane
        { 2,  new ProductContract { ProductId = 2,  Price = 1.50, VatRate = 0.10 } },  // Latte
        { 3,  new ProductContract { ProductId = 3,  Price = 3.00, VatRate = 0.10 } },  // Pasta

        // Libri (IVA 4%)
        { 4,  new ProductContract { ProductId = 4,  Price = 15.90, VatRate = 0.04 } }, // Romanzo
        { 5,  new ProductContract { ProductId = 5,  Price = 28.00, VatRate = 0.04 } }, // Manuale tecnico

        // Elettronica (IVA 22%)
        { 6,  new ProductContract { ProductId = 6,  Price = 399.99, VatRate = 0.22 } }, // Smartphone
        { 7,  new ProductContract { ProductId = 7,  Price = 899.00, VatRate = 0.22 } }, // Laptop

        // Giocattoli (IVA 22%)
        { 8,  new ProductContract { ProductId = 8,  Price = 24.95, VatRate = 0.22 } }, // Puzzle 1000 pezzi
        { 9,  new ProductContract { ProductId = 9,  Price = 49.90, VatRate = 0.22 } }, // Action figure

        // Abbigliamento (IVA 22%)
        { 10, new ProductContract { ProductId = 10, Price = 59.99, VatRate = 0.22 } }  // Felpa
    };

    public OrderResponse CreateOrder(OrderRequest request)
    {
        var response = new OrderResponse
        {
            OrderId = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Items = new List<OrderItemResponse>()
        };

        foreach (var item in request.Items)
        {
            if (!_catalog.TryGetValue(item.ProductId, out var product))
                continue;

            var subtotal = product.Price * item.Quantity;
            var vat = subtotal * product.VatRate;

            response.Items.Add(new OrderItemResponse
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = Math.Round(subtotal, 2),
                Vat = Math.Round(vat, 2)
            });

            response.OrderPrice += subtotal;
            response.OrderVat += vat;
        }

        response.OrderPrice = Math.Round(response.OrderPrice, 2);
        response.OrderVat = Math.Round(response.OrderVat, 2);

        return response;
    }
}
*/