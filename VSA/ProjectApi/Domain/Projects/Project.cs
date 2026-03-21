using ProjectApi.Domain.Common;
using ProjectApi.Domain.Enums;
using ProjectApi.Domain.Persons;
using ProjectApi.Domain.Projects.Events;
using ProjectApi.Domain.ValueObjects;

namespace ProjectApi.Domain.Projects;

public sealed class Project : AggregateRoot
{
    private readonly List<ProjectTask> _tasks = [];
    private readonly List<ProjectAssignment> _assignments = [];

    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateRange DateRange { get; private set; }
    public ProjectStatus Status { get; private set; }

    public IReadOnlyList<ProjectTask> Tasks => _tasks.AsReadOnly();
    public IReadOnlyList<ProjectAssignment> Assignments => _assignments.AsReadOnly();

    // EF Core için parametre-siz constructor
    private Project() : base()
    {
        Name = string.Empty;
        Description = string.Empty;
        DateRange = DateRange.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
    }

    private Project(
        Guid id,
        string name,
        string description,
        DateRange dateRange) : base(id)
    {
        Name = name;
        Description = description;
        DateRange = dateRange;
        Status = ProjectStatus.Proposed;
    }

    // --- Factory Method ---

    public static Project Create(
        string name,
        string description,
        DateTime startDate,
        DateTime endDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Project name cannot be empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.", nameof(description));

        var dateRange = DateRange.Create(startDate, endDate);
        var project = new Project(Guid.NewGuid(), name, description, dateRange);

        project.AddDomainEvent(new ProjectCreatedEvent(project.Id, project.Name));

        return project;
    }

    // --- Durum Yönetimi ---

    public void Activate()
    {
        if (Status != ProjectStatus.Proposed)
            throw new InvalidOperationException("Only proposed projects can be activated.");

        Status = ProjectStatus.Active;
    }

    public void Complete()
    {
        if (Status != ProjectStatus.Active)
            throw new InvalidOperationException("Only active projects can be completed.");

        Status = ProjectStatus.Completed;
    }

    public void Cancel()
    {
        if (Status == ProjectStatus.Completed)
            throw new InvalidOperationException("Completed projects cannot be cancelled.");

        Status = ProjectStatus.Cancelled;
    }

    // --- Proje Detayları ---

    public void UpdateDetails(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Project name cannot be empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.", nameof(description));

        Name = name;
        Description = description;
    }

    public void UpdateDateRange(DateTime startDate, DateTime endDate)
    {
        DateRange = DateRange.Create(startDate, endDate);
    }

    // --- Personel Yönetimi ---

    public ProjectAssignment AssignPerson(Person person, string role)
    {
        ArgumentNullException.ThrowIfNull(person);

        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role cannot be empty.", nameof(role));

        if (_assignments.Any(a => a.PersonId == person.Id))
            throw new InvalidOperationException("Person is already assigned to this project.");

        var assignment = new ProjectAssignment(Id, person.Id, role);
        _assignments.Add(assignment);

        AddDomainEvent(new PersonnelAssignedEvent(Id, person.Id, role));

        return assignment;
    }

    public void RemoveAssignment(Guid personId)
    {
        var assignment = _assignments.FirstOrDefault(a => a.PersonId == personId)
            ?? throw new InvalidOperationException("Person is not assigned to this project.");

        _assignments.Remove(assignment);
    }

    // --- Görev Yönetimi ---

    public ProjectTask AddTask(
        string title,
        string description,
        TaskPriority priority,
        DateTime? dueDate = null)
    {
        if (Status is ProjectStatus.Completed or ProjectStatus.Cancelled)
            throw new InvalidOperationException("Cannot add tasks to a completed or cancelled project.");

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Task title cannot be empty.", nameof(title));

        var task = new ProjectTask(Id, title, description, priority, dueDate);
        _tasks.Add(task);

        AddDomainEvent(new TaskAddedToProjectEvent(Id, task.Id, task.Title));

        return task;
    }

    public void RemoveTask(Guid taskId)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == taskId)
            ?? throw new InvalidOperationException("Task not found in project.");

        if (task.Status == ProjectTaskStatus.InProgress)
            throw new InvalidOperationException("Cannot remove an in-progress task.");

        _tasks.Remove(task);
    }

    // --- Sorgular ---

    public bool IsPersonAssigned(Guid personId) =>
        _assignments.Any(a => a.PersonId == personId);

    public bool IsActive() =>
        DateRange.IsActive() && Status == ProjectStatus.Active;

    public int GetTaskCount() => _tasks.Count;
    public int GetAssignmentCount() => _assignments.Count;
}
