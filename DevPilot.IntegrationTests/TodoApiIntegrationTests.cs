using System.Net.Http.Json;
using DevPilot.Api;
using DevPilot.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class TodoApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public TodoApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact(Skip = "Requires running DB and auth setup")]
    public async Task GetAll_Unauthorized_Returns401()
    {
        var response = await _client.GetAsync("/api/TodoItems");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }
} 