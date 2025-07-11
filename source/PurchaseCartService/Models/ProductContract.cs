using System.ComponentModel.DataAnnotations;

namespace PurchaseCartService.Models;

public class ProductContract
{
    [Key]
    public int ProductId { get; set; }
    public double Price { get; set; }
    public double VatRate { get; set; }
}