using Microsoft.Net.Http.Headers;

namespace Buildforge.Service.Controller.Event.V1;

[ApiController]
[Route("api/v1/events")]
[ApiExplorerSettings(GroupName = "v1")]
public class EventController(EventPublisher buildSubscriber) : ControllerBase
{
    [HttpGet("subscribeSse")]
    public async Task SubscribeSse(CancellationToken ct)
    {
        await using var subscription = await buildSubscriber.Subscribe();

        HttpContext.Response.Headers.Append(HeaderNames.ContentType, "text/event-stream");

        while (!ct.IsCancellationRequested)
        {
            await foreach (var item in subscription.ChannelReader.ReadAllAsync(ct))
            {
                await HttpContext.Response.WriteAsync($"event:{item.GetType().Name}\n", cancellationToken: ct);

                await HttpContext.Response.WriteAsync($"data:", cancellationToken: ct);

                await JsonSerializer.SerializeAsync(HttpContext.Response.Body, Event.V1.Model.Event.FromDomain(item), cancellationToken: ct);

                await HttpContext.Response.WriteAsync($"\n\n", cancellationToken: ct);

                await HttpContext.Response.Body.FlushAsync(ct);
            }
        }
    }

    [HttpGet("subscribe")]
    public async IAsyncEnumerable<Model.Event> Subscribe([EnumeratorCancellation] CancellationToken ct)
    {
        await using var subscription = await buildSubscriber.Subscribe();

        while (!ct.IsCancellationRequested)
        {
            await foreach (var item in subscription.ChannelReader.ReadAllAsync(ct))
            {
                var model = Event.V1.Model.Event.FromDomain(item);

                if (model is not null)
                {
                    yield return model;
                }
            }
        }
    }
}