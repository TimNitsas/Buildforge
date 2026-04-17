using RandomFriendlyNameGenerator;

namespace Buildforge.Service.Provider.Job.Simulation;

public sealed class JobProviderSimulatorItem
{
    private static readonly Random Random = new(42);

    private static readonly List<string> Platforms =
    [
        "Pc",
        "Playstation",
        "Xbox",
    ];

    private readonly DateTime Inception;

    private readonly DateTime TimeInQueue;

    private readonly DateTime TimeToCompletion;

    private readonly Job Job;

    public JobProviderSimulatorItem(int index)
    {
        Inception = DateTime.UtcNow;

        TimeInQueue = Inception.AddSeconds(Random.Next(10, 60));

        TimeToCompletion = TimeInQueue.AddSeconds(Random.Next(10, 60));

        Job = new Job()
        {
            StartTime = Inception,
            Id = $"cl-{index}",
            Branch = "Main",
            Name = NameGenerator.Identifiers.Get(),
            Platform = Platforms[Random.Next(Platforms.Count)],
            Target = "release",
            Status = new JobStatusQueued()
            {
                StartTime = Inception,
            }
        };
    }

    public Job Resolve()
    {
        var now = DateTime.UtcNow;

        if (now > TimeInQueue)
        {
            if (now > TimeToCompletion)
            {
                if (Job.Status is JobStatusActive a)
                {
                    if (Random.NextDouble() >= 0.5f)
                    {
                        Job.Status = new JobStatusFailed()
                        {
                            StartTime = a.StartTime,
                            Reason = "<some_reason>",
                        };
                    }
                    else
                    {
                        Job.Status = new JobStatusSuccess()
                        {
                            StartTime = a.StartTime,
                            BuildTime = now - a.StartTime,
                            Bytes = 1024 * 1024 * 1024 * (long)Random.Next(20, 30)
                        };
                    }
                }
            }
            else
            {
                Job.Status = new JobStatusActive()
                {
                    StartTime = Inception,
                    EstimatedTimeToCompletion = TimeToCompletion
                };
            }
        }

        return Job;
    }
}