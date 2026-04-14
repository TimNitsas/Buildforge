namespace Buildforge.Service.Provider.Job;

public sealed class Job
{
    public required DateTime StartTime { get; set; }

    public required JobStatus Status { get; set; }

    public required string Id { get; set; }

    public required string Name { get; set; }

    public required string Target { get; set; }

    public required string Platform { get; set; }

    public required string Branch { get; set; }
}