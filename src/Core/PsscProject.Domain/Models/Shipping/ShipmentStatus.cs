namespace PsscProject.Domain.Models.Shipping;

/// <summary>
/// Shipment status enumeration
/// </summary>
public enum ShipmentStatus
{
    Created = 0,
    Packed = 1,
    InTransit = 2,
    Delivered = 3,
    Cancelled = 4
}
