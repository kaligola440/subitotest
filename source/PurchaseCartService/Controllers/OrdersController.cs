using Microsoft.AspNetCore.Mvc;
using PurchaseCartService.Models.Requests;
using PurchaseCartService.Models.Responses;
using PurchaseCartService.Services;

namespace PurchaseCartService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _service;

    public OrdersController(OrderService service)
    {
        _service = service; // Usa il servizio che contiene la logica
    }

    [HttpPost]
    public ActionResult<OrderResponse> CreateOrder([FromBody] OrderRequest request)
    {
        if (request == null || request.Items == null || request.Items.Count == 0)
        {
            return BadRequest("Invalid order data.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var response = _service.CreateOrder(request);
            return Ok(response);
        }
        
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
