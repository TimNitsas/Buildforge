using System.Runtime.CompilerServices;

namespace Buildforge.Client.V1;

public partial class MockEventClient : IEventClient
{
    public Task<ICollection<Event>> SubscribeAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<ICollection<Event>>([]);
    }

    public Task SubscribeSseAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public async IAsyncEnumerable<Event> SubscribeSseImpl([EnumeratorCancellation] CancellationToken ct)
    {
        await Task.Yield();

        yield break;
    }
}