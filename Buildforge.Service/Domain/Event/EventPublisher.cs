using System.Threading.Channels;

namespace Buildforge.Service.Domain.Event;

public sealed class EventPublisher
{
    private readonly SemaphoreSlim Semaphore = new(1, 1);

    private readonly Dictionary<EventSubscription, Channel<Model.BaseEvent>> Channels = [];

    public async Task<EventSubscription> Subscribe()
    {
        try
        {
            await Semaphore.WaitAsync();

            var channel = Channel.CreateUnbounded<Model.BaseEvent>(new UnboundedChannelOptions()
            {
                AllowSynchronousContinuations = false,
                SingleReader = true,
                SingleWriter = true
            });

            var subscription = new EventSubscription(this, channel.Reader);

            Channels.Add(subscription, channel);

            return subscription;
        }
        finally
        {
            Semaphore.Release();
        }
    }

    public async Task Unsubscribe(EventSubscription subscription)
    {
        try
        {
            await Semaphore.WaitAsync();

            Channels.Remove(subscription);
        }
        finally
        {
            Semaphore.Release();
        }
    }

    public async Task Publish(Model.BaseEvent e)
    {
        try
        {
            await Semaphore.WaitAsync();

            foreach (var channel in Channels.Values)
            {
                channel.Writer.TryWrite(e);
            }
        }
        finally
        {
            Semaphore.Release();
        }
    }
}