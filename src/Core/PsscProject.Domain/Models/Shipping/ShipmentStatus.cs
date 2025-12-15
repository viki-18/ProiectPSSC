namespace PsscProject.Domain.Models.Shipping;

public enum ShipmentStatus
{
    Created = 0,
    Packed = 1,
    InTransit = 2,
    Delivered = 3,
    Cancelled = 4
}
