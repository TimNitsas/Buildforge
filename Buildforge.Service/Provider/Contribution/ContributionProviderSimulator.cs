using Bogus;

namespace Buildforge.Service.Provider.Contribution;

public sealed class ContributionProviderSimulator : IContributionProvider
{
    private readonly SemaphoreSlim Semaphore = new(1, 1);

    private readonly List<Contribution> Contributions = [];

    public ContributionProviderSimulator()
    {
        Simulate();
    }

    public async IAsyncEnumerable<Contribution> GetContributions([EnumeratorCancellation] CancellationToken ct)
    {
        try
        {
            await Semaphore.WaitAsync(ct);

            foreach (var item in Contributions)
            {
                yield return item;
            }
        }
        finally
        {
            Semaphore.Release();
        }
    }

    private void Simulate()
    {
        Task.Run(async () =>
        {
            var faker = new Faker();

            while (true)
            {
                try
                {
                    await Semaphore.WaitAsync();

                    Contributions.Add(new Contribution()
                    {
                        User = faker.Name.FullName(),
                        Description = faker.Lorem.Sentences(2),
                        Id = $"{Contributions.Count}"
                    });

                    await Task.Delay(1_000);
                }
                finally
                {
                    Semaphore.Release();
                }
            }
        });
    }
}