using System.Text.Json.Serialization;

namespace PurchaseCartService.Models.Responses;

public class OrderItemResponse
{
    [JsonPropertyName("product_id")]
    public int ProductId { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("vat")]
    public double Vat { get; set; }
}

public class OrderResponse
{
    [JsonPropertyName("order_id")]
    public long OrderId { get; set; }

    [JsonPropertyName("order_price")]
    public double OrderPrice { get; set; }

    [JsonPropertyName("order_vat")]
    public double OrderVat { get; set; }

    [JsonPropertyName("items")]
    public List<OrderItemResponse> Items { get; set; } = new();
}