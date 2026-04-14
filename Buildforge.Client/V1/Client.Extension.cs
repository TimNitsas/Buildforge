using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;

namespace Buildforge.Client.V1;

public partial interface IEventClient
{
    IAsyncEnumerable<Event> SubscribeSseImpl(CancellationToken ct);
}

public partial class EventClient : IEventClient
{
    public async IAsyncEnumerable<Event> SubscribeSseImpl([EnumeratorCancellation] CancellationToken ct)
    {
        using var stream = await _httpClient.GetStreamAsync($"{_baseUrl}api/v1/events/subscribeSse", ct);

        await foreach (SseItem<string> item in SseParser.Create(stream).EnumerateAsync(ct))
        {
            var e = Newtonsoft.Json.JsonConvert.DeserializeObject<Event>(item.Data, JsonSerializerSettings);

            if (e is not null)
            {
                yield return e;
            }
        }
    }
}