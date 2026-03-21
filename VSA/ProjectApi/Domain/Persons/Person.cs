using ProjectApi.Domain.Common;
using ProjectApi.Domain.ValueObjects;

namespace ProjectApi.Domain.Persons;

public sealed class Person : AggregateRoot
{
    public string Title { get; private set; }
    public string FullName { get; private set; }
    public EmailAddress Email { get; private set; }

    // EF Core için parametre-siz constructor
    private Person() : base()
    {
        Title = string.Empty;
        FullName = string.Empty;
        Email = EmailAddress.Create("placeholder@example.com");
    }

    private Person(Guid id, string title, string fullName, EmailAddress email) : base(id)
    {
        Title = title;
        FullName = fullName;
        Email = email;
    }

    public static Person Create(string title, string fullName, string email)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));

        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be empty.", nameof(fullName));

        var emailVO = EmailAddress.Create(email);

        return new Person(Guid.NewGuid(), title, fullName, emailVO);
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));

        Title = title;
    }

    public void UpdateFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be empty.", nameof(fullName));

        FullName = fullName;
    }

    public void UpdateEmail(string email)
    {
        Email = EmailAddress.Create(email);
    }
}
