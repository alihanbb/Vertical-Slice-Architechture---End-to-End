using ProjectApi.Domain.Common;
using ProjectApi.Domain.Enums;
using ProjectApi.Domain.Persons;

namespace ProjectApi.Domain.Projects;

public sealed class ProjectTask : Entity
{
    public Guid ProjectId { get; private set; }
    public Guid? AssignedToId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public TaskPriority Priority { get; private set; }
    public ProjectTaskStatus Status { get; private set; }
    public DateTime? DueDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // EF Core navigation property
    public Person? AssignedTo { get; private set; }

    // EF Core için parametre-siz constructor
    private ProjectTask() : base()
    {
        Title = string.Empty;
        Description = string.Empty;
    }

    internal ProjectTask(
        Guid projectId,
        string title,
        string description,
        TaskPriority priority,
        DateTime? dueDate) : base(Guid.NewGuid())
    {
        ProjectId = projectId;
        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        CreatedAt = DateTime.UtcNow;
        Status = ProjectTaskStatus.ToDo;
    }

    public void AssignTo(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);
        AssignedToId = person.Id;
        AssignedTo = person;
    }

    public void Unassign()
    {
        AssignedToId = null;
        AssignedTo = null;
    }

    public void UpdateStatus(ProjectTaskStatus newStatus)
    {
        if (Status == ProjectTaskStatus.Done && newStatus != ProjectTaskStatus.Done)
            throw new InvalidOperationException("Cannot reopen a completed task.");

        Status = newStatus;
    }

    public void UpdateDetails(
        string title,
        string description,
        TaskPriority priority,
        DateTime? dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Task title cannot be empty.", nameof(title));

        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
    }
}
