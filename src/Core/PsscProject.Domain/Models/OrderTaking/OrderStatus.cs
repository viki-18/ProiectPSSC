namespace PsscProject.Domain.Models.OrderTaking
{
    public enum OrderStatus
    {
        Pending = 0,
        Placed = 1,
        Invoiced = 2,
        Shipped = 3,
        Cancelled = 4
    }
}