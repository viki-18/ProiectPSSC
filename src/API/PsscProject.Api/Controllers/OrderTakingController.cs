using Microsoft.AspNetCore.Mvc;
using PsscProject.Application.Workflows.OrderTaking;
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderTakingController : ControllerBase
{
    private readonly PlaceOrderWorkflow _placeOrderWorkflow;

    // Dependency Injection - workflow-ul va fi injectat din container
    public OrderTakingController(PlaceOrderWorkflow placeOrderWorkflow)
    {
        _placeOrderWorkflow = placeOrderWorkflow;
    }

    /// <summary>
    /// POST /api/ordertaking/place-order
    /// Preluare comandă - triggers Place Order workflow
    /// </summary>
    [HttpPost("place-order")]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand command)
    {
        try
        {
            var result = await _placeOrderWorkflow.ExecuteAsync(command);

            return Ok(new
            {
                success = true,
                orderId = result.OrderId,
                customerId = result.CustomerId,
                totalAmount = result.TotalAmount,
                currency = result.Currency,
                lineCount = result.LineCount,
                timestamp = result.Timestamp
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                success = false,
                error = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                error = "Internal server error: " + ex.Message
            });
        }
    }
}
