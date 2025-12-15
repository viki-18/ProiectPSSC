using Microsoft.AspNetCore.Mvc;
using PsscProject.Application.Workflows.OrderTaking;
using PsscProject.Domain.Models.OrderTaking; // Asigură-te că ai acest using
using PsscProject.Domain.Models; // Pentru a recunoaște clasa 'Result'
namespace PsscProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderTakingController : ControllerBase
    {
        private readonly PlaceOrderWorkflow _placeOrderWorkflow;
        private readonly CancelOrderWorkflow _cancelOrderWorkflow;

        public OrderTakingController(
            PlaceOrderWorkflow placeOrderWorkflow,
            CancelOrderWorkflow cancelOrderWorkflow)
        {
            _placeOrderWorkflow = placeOrderWorkflow;
            _cancelOrderWorkflow = cancelOrderWorkflow;
        }

        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand command)
        {
            var result = await _placeOrderWorkflow.ExecuteAsync(command);

            if (result.IsFailure)
            {
                return BadRequest(new { error = result.Error });
            }

            return Ok(result.Value);
        }

        [HttpPost("cancel-order")]
        public async Task<IActionResult> CancelOrder([FromBody] CancelOrderCommand command)
        {
            var result = await _cancelOrderWorkflow.ExecuteAsync(command);

            if (result.IsFailure)
            {
                return BadRequest(new { error = result.Error });
            }

            return Ok(new 
            { 
                message = "Order cancelled successfully", 
                orderId = result.Value.OrderId 
            });
        }
    }
}