namespace Buildforge.App.Domain.Build;

public sealed class BuildHandlerDownloadProgressEvent
{
    public required string BuildId { get; init; }

    public required long BytesReceived { get; init; }

    public required long TotalBytes { get; init; }

    public required double ProgressPercentage { get; init; }

    public required double SpeedKbps { get; init; }

    public TimeSpan EstimatedEta { get; init; }
}