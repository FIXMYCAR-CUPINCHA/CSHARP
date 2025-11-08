using System.Net.Http.Json;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;

public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public IntegrationTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async System.Threading.Tasks.Task Health_Check_Returns_OK()
    {
        var client = _factory.CreateClient();
        var res = await client.GetAsync("/health");
        res.EnsureSuccessStatusCode();
    }

    [Fact]
    public async System.Threading.Tasks.Task Can_Get_Jwt_Token_And_Access_Protected()
    {
        var client = _factory.CreateClient();
        var tokenResp = await client.PostAsJsonAsync("/api/v1/auth/token", new { Username = "admin", Password = "password" });
        tokenResp.EnsureSuccessStatusCode();
        var obj = await tokenResp.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
        var token = obj.GetProperty("token").GetString();
        token.Should().NotBeNullOrEmpty();
    }
}