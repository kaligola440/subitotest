using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PurchaseCartService.Models.Requests;

public class OrderItem
{
    /// <summary>
    /// Product Id
    /// </summary>
    [Required]
    [JsonPropertyName("product_id")]
    public int ProductId { get; set; }

    /// <summary>
    /// Product Quantity
    /// </summary>
    [Required]
    [JsonPropertyName("quantity")]
    [Range(1, int.MaxValue, ErrorMessage = "Product quantity must be greater than 0")]
    public int Quantity { get; set; }
}

public class OrderRequest
{
    [Required]
    public List<OrderItem> Items { get; set; } = new();
}