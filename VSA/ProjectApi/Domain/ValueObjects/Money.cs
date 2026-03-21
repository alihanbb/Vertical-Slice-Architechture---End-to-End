namespace ProjectApi.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, string currency = "TRY")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty.");

        return new Money(amount, currency);
    }

    public static Money Zero(string currency = "TRY") => new(0, currency);

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Currencies must match for addition.");
        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money operator -(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Currencies must match for subtraction.");

        if (left.Amount < right.Amount)
            throw new InvalidOperationException("Result cannot be negative.");

        return new Money(left.Amount - right.Amount, left.Currency);
    }

    public override string ToString() => $"{Amount:F2} {Currency}";
}
