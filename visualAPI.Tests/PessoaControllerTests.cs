using System.Net;
using System.Net.Http.Json;
using Xunit;

public class PessoaControllerTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PessoaControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_Pessoas_ShouldReturn200()
    {
        var response = await _client.GetAsync("/api/pessoa");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
