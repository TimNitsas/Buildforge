using Buildforge.Service.Test.Build.Core;
using Microsoft.AspNetCore.WebUtilities;

namespace Buildforge.Service.Test.Build;

public class BuildControllerTest(BuildforgeWebApplicationFactory<Program> factory) : IClassFixture<BuildforgeWebApplicationFactory<Program>>
{
    [Fact]
    public async Task Can_Get_Builds()
    {
        var response = await factory.CreateClient().GetAsync("/api/v1/builds/", TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Can_Download_Build()
    {
        var query = new Dictionary<string, string?>
        {
            ["buildId"] = "id"
        };

        var url = QueryHelpers.AddQueryString("/api/v1/builds/download", query);

        var response = await factory.CreateClient().GetAsync(url, HttpCompletionOption.ResponseHeadersRead, TestContext.Current.CancellationToken);

        Assert.Equal("application/octet-stream", response.Content.Headers.ContentType!.MediaType);

        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(TestContext.Current.CancellationToken);

        using var memory = new MemoryStream();

        await stream.CopyToAsync(memory, TestContext.Current.CancellationToken);

        Assert.True(memory.Length > 0, "Downloaded file should not be empty.");
    }
}