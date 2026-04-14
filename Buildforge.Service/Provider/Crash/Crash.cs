namespace Buildforge.Service.Provider.Crash;

public sealed class Crash
{
    public required string CrashId { get; set; }

    public required string User { get; init; }

    public required DateTime Date { get; init; }

    public required TimeSpan PlayTime { get; init; }

    public required string BuildId { get; init; }
}