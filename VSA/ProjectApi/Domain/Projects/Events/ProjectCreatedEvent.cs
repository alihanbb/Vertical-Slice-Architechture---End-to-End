namespace ProjectApi.Domain.Projects.Events;

public sealed record ProjectCreatedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Guid ProjectId { get; }
    public string ProjectName { get; }

    public ProjectCreatedEvent(Guid projectId, string projectName)
    {
        ProjectId = projectId;
        ProjectName = projectName;
    }
}
