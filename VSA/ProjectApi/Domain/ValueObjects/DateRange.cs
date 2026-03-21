namespace ProjectApi.Domain.ValueObjects;

public sealed record DateRange
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    private DateRange(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public static DateRange Create(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be after end date.");

        return new DateRange(startDate, endDate);
    }

    public int DurationInDays => (EndDate - StartDate).Days;

    public bool Contains(DateTime date) => date >= StartDate && date <= EndDate;

    public bool Overlaps(DateRange other) =>
        StartDate <= other.EndDate && EndDate >= other.StartDate;

    public bool IsActive() => Contains(DateTime.UtcNow);

    public override string ToString() =>
        $"{StartDate:yyyy-MM-dd} → {EndDate:yyyy-MM-dd} ({DurationInDays} days)";
}
