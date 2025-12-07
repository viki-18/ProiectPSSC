namespace PsscProject.Domain.Models.Invoicing;

/// <summary>
/// Invoice status enumeration
/// </summary>
public enum InvoiceStatus
{
    Draft = 0,
    Issued = 1,
    Paid = 2,
    Cancelled = 3
}
