using Bogus;
using Buildforge.Service.Repository.Build;
using System.Collections.Concurrent;

namespace Buildforge.Service.Provider.Crash.Simulation;

public class CrashProviderSimulator : ICrashProvider
{
    private readonly ConcurrentDictionary<string, List<Crash>> Crashes = [];

    public CrashProviderSimulator(BuildRepository buildProvider)
    {
        SimulateCrashes(buildProvider);
    }

    private void SimulateCrashes(BuildRepository buildProvider)
    {
        Task.Run(async () =>
        {
            var faker = new Faker();

            await buildProvider.Initialize(CancellationToken.None);

            var builds = await buildProvider.GetBuilds(CancellationToken.None).ToListAsync();

            var random = new Random(42);

            while (true)
            {
                foreach (var build in builds)
                {
                    if (random.NextDouble() <= 0.90f)
                    {
                        continue;
                    }

                    Crash crash = new()
                    {
                        Date = DateTime.Now,
                        PlayTime = TimeSpan.FromMinutes(random.Next(1, 60 * 2)),
                        User = faker.Name.FullName(),
                        BuildId = build.Id,
                        CrashId = Guid.NewGuid().ToString()
                    };

                    Crashes.TryAdd(build.Id, []);

                    Crashes[build.Id].Add(crash);
                }

                await Task.Delay(5_000);
            }
        });
    }

    public IEnumerable<Crash> GetCrashes(CancellationToken ct)
    {
        foreach (var builds in Crashes.Values)
        {
            foreach (var buildCrash in builds)
            {
                yield return buildCrash;
            }
        }
    }
}