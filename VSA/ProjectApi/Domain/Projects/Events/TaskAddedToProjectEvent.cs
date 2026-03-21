namespace ProjectApi.Domain.Projects.Events;

public sealed record TaskAddedToProjectEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Guid ProjectId { get; }
    public Guid TaskId { get; }
    public string TaskTitle { get; }

    public TaskAddedToProjectEvent(Guid projectId, Guid taskId, string taskTitle)
    {
        ProjectId = projectId;
        TaskId = taskId;
        TaskTitle = taskTitle;
    }
}
