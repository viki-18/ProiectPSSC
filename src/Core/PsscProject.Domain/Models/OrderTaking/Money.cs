namespace PsscProject.Domain.Models.OrderTaking;

public record Money(decimal Amount, string Currency = "USD")
{
    public static Money Zero => new(0m, "USD");

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Cannot add money with different currencies");
        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Money operator *(Money m, int quantity)
    {
        return new Money(m.Amount * quantity, m.Currency);
    }
}
