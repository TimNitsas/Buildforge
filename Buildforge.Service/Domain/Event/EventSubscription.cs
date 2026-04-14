using System.Threading.Channels;

namespace Buildforge.Service.Domain.Event;

public sealed class EventSubscription(EventPublisher owner, ChannelReader<Model.BaseEvent> channelReader) : IAsyncDisposable
{
    public ChannelReader<Model.BaseEvent> ChannelReader { get; } = channelReader;

    public async ValueTask DisposeAsync()
    {
        await owner.Unsubscribe(this);
    }
}