using System.Text.RegularExpressions;

namespace ProjectApi.Domain.ValueObjects;

public sealed record EmailAddress
{
    public string Value { get; }

    private EmailAddress(string value) => Value = value;

    public static EmailAddress Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.");

        if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Invalid email format.");

        return new EmailAddress(value.ToLowerInvariant());
    }

    public static implicit operator string(EmailAddress email) => email.Value;
    public override string ToString() => Value;
}
