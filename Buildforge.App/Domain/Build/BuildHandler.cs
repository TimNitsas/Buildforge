using Buildforge.Client.V1;
using CommunityToolkit.Mvvm.Messaging;
using System.IO.Pipelines;
using System.Net.Http.Headers;

namespace Buildforge.App.Domain.Build;

public sealed class BuildHandler(IBuildClient buildClient, IMessenger messenger, BuildLocator buildLocator)
{
    public async Task DownloadBuild(string buildId, CancellationToken ct)
    {
        var response = await buildClient.DownloadBuildAsync(buildId, ct);

        long totalBytes = long.Parse(response.Headers["Content-Length"].First());

        var contentDisposition = ContentDispositionHeaderValue.Parse(response.Headers["Content-Disposition"].First());

        var path = Path.Combine(buildLocator.RootFolder, contentDisposition.FileName!);

        using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);

        var stopwatch = Stopwatch.StartNew();

        using var stream = response.Stream;

        await DownloadImpl(buildId, response.Stream, fileStream, totalBytes, stopwatch, ct);

        messenger.Send(new BuildHandlerCompletionEvent()
        {
            Id = buildId,
            Time = stopwatch.Elapsed
        });
    }

    public async Task DownloadImpl(string buildId, Stream input, Stream output, long totalBytes, Stopwatch stopwatch, CancellationToken ct)
    {
        var reader = PipeReader.Create(input);

        var writer = PipeWriter.Create(output);

        long totalBytesRead = 0;

        try
        {
            while (true)
            {
                var result = await reader.ReadAsync(ct);

                var buffer = result.Buffer;

                foreach (var segment in buffer)
                {
                    await writer.WriteAsync(segment, ct);

                    totalBytesRead += segment.Length;

                    ReportProgress(buildId, totalBytesRead, totalBytes, stopwatch);
                }

                reader.AdvanceTo(buffer.End);

                if (result.IsCompleted)
                {
                    break;
                }
            }

            await writer.FlushAsync(ct);
        }
        finally
        {
            await reader.CompleteAsync();

            await writer.CompleteAsync();
        }
    }

    private void ReportProgress(string buildId, long totalRead, long totalBytes, Stopwatch stopwatch)
    {
        var speedKbps = totalRead / 1024d / stopwatch.Elapsed.TotalSeconds;

        var progress = totalRead / (double)totalBytes * 100;

        var remainingBytes = totalBytes - totalRead;

        var etaSeconds = speedKbps > 0 ? remainingBytes / (speedKbps * 1024d) : 0;

        messenger.Send<BuildHandlerDownloadProgressEvent>(new BuildHandlerDownloadProgressEvent
        {
            BuildId = buildId,
            BytesReceived = totalRead,
            TotalBytes = totalBytes,
            ProgressPercentage = progress,
            SpeedKbps = speedKbps,
            EstimatedEta = TimeSpan.FromSeconds(etaSeconds)
        });
    }
}