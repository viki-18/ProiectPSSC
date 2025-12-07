using Microsoft.AspNetCore.Mvc;
using PsscProject.Application.Workflows.Shipping;
using PsscProject.Domain.Interfaces.Shipping;
using PsscProject.Domain.Models.Shipping;
using PsscProject.Domain.Models.OrderTaking;

namespace PsscProject.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShippingController : ControllerBase
{
    private readonly CreateShipmentWorkflow _createShipmentWorkflow;
    private readonly IShipmentsRepository _shipmentsRepository;

    public ShippingController(
        CreateShipmentWorkflow createShipmentWorkflow,
        IShipmentsRepository shipmentsRepository)
    {
        _createShipmentWorkflow = createShipmentWorkflow;
        _shipmentsRepository = shipmentsRepository;
    }

    /// <summary>
    /// POST /api/shipping/create-shipment
    /// Creare expediere pentru o comandă existentă
    /// </summary>
    [HttpPost("create-shipment")]
    public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentRequest request)
    {
        try
        {
            var result = await _createShipmentWorkflow.ExecuteAsync(
                new OrderId(request.OrderId)
            );

            return Ok(new
            {
                success = true,
                shipmentId = result.ShipmentId,
                orderId = result.OrderId,
                customerId = result.CustomerId,
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

    /// <summary>
    /// GET /api/shipping/{shipmentId}
    /// Preia detaliile unei expedieri
    /// </summary>
    [HttpGet("{shipmentId}")]
    public async Task<IActionResult> GetShipment(Guid shipmentId)
    {
        try
        {
            var shipment = await _shipmentsRepository.GetShipmentByIdAsync(
                new ShipmentId(shipmentId)
            );

            if (shipment == null)
                return NotFound(new { error = "Shipment not found" });

            return Ok(new
            {
                shipmentId = shipment.Id.Value,
                orderId = shipment.OrderId.Value,
                customerId = shipment.CustomerId.Value,
                status = shipment.Status.ToString(),
                createdAt = shipment.CreatedAt,
                lines = shipment.Lines.Select(l => new
                {
                    productId = l.ProductId.Value,
                    productName = l.ProductName,
                    quantity = l.Quantity
                }).ToList()
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

public record CreateShipmentRequest(Guid OrderId);
