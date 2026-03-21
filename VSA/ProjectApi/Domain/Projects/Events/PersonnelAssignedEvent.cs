namespace ProjectApi.Domain.Projects.Events;

public sealed record PersonnelAssignedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Guid ProjectId { get; }
    public Guid PersonId { get; }
    public string Role { get; }

    public PersonnelAssignedEvent(Guid projectId, Guid personId, string role)
    {
        ProjectId = projectId;
        PersonId = personId;
        Role = role;
    }
}
