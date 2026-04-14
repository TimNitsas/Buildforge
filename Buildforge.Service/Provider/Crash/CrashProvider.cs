namespace Buildforge.Service.Provider.Crash;

public interface ICrashProvider
{
    IEnumerable<Crash> GetCrashes(CancellationToken ct);
}