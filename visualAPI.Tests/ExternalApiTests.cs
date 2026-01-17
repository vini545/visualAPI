using System.Net;
using System.Net.Http.Json;
using Xunit;

public class ExternalApiTests
{
    private readonly HttpClient _client;

    public ExternalApiTests()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com/")
        };
    }

    [Fact]
    public async Task Get_Posts_ShouldReturn200AndData()
    {
        // Act
        var response = await _client.GetAsync("posts");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(content));
    }
}
