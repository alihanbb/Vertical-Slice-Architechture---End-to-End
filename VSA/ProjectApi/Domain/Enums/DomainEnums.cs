namespace ProjectApi.Domain.Enums;

public enum ProjectStatus
{
    Proposed,
    Active,
    Completed,
    Cancelled
}

public enum TaskPriority
{
    Low,
    Medium,
    High,
    Critical
}

public enum ProjectTaskStatus
{
    ToDo,
    InProgress,
    Review,
    Done
}
