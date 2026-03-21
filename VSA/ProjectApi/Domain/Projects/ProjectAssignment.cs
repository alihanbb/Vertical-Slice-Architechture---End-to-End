using ProjectApi.Domain.Common;
using ProjectApi.Domain.Persons;

namespace ProjectApi.Domain.Projects;

public sealed class ProjectAssignment : Entity
{
    public Guid ProjectId { get; private set; }
    public Guid PersonId { get; private set; }
    public string Role { get; private set; }
    public DateTime AssignedAt { get; private set; }

    // EF Core navigation property
    public Person Person { get; private set; } = null!;

    // EF Core için parametre-siz constructor
    private ProjectAssignment() : base()
    {
        Role = string.Empty;
    }

    internal ProjectAssignment(Guid projectId, Guid personId, string role) : base(Guid.NewGuid())
    {
        ProjectId = projectId;
        PersonId = personId;
        Role = role;
        AssignedAt = DateTime.UtcNow;
    }

    public void UpdateRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role cannot be empty.", nameof(role));

        Role = role;
    }
}
