using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace VirGo.SecurityTests;

public sealed class InputSecurityTests(SecurityApiFactory factory) : IClassFixture<SecurityApiFactory>
{
    private static readonly string Long = new('a', 1500);
    private readonly HttpClient client = factory.CreateClient();

    [Theory]
    [InlineData("name")]
    [InlineData("email")]
    [InlineData("password")]
    [InlineData("phone")]
    public async Task RegisterRejectsOversizedFields(string field)
    {
        var body = new Dictionary<string, string>
        {
            ["name"] = "Ana López",
            ["email"] = $"{Guid.NewGuid():N}@virgo.test",
            ["password"] = "Password-Segura-1",
            ["phone"] = "+524421234567",
            [field] = Long
        };

        var response = await client.PostAsJsonAsync("/api/v1/auth/register", body);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData(100, HttpStatusCode.Created)]
    [InlineData(101, HttpStatusCode.BadRequest)]
    public async Task RegisterEnforcesTheExactNameLimit(int length, HttpStatusCode expected)
    {
        var response = await client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            name = new string('a', length),
            email = $"{Guid.NewGuid():N}@virgo.test",
            password = "Password-Segura-1",
            phone = "+524421234567"
        });

        Assert.Equal(expected, response.StatusCode);
    }

    [Fact]
    public async Task LoginRejectsOversizedFieldsWithoutServerError()
    {
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", new { email = Long, password = Long });

        Assert.True((int)response.StatusCode < 500, response.StatusCode.ToString());
    }

    [Theory]
    [InlineData("{\"email\":{\"$ne\":null},\"password\":\"x\"}")]
    [InlineData("{\"email\":\"' OR 1=1 --\",\"password\":\"x\"}")]
    [InlineData("{\"email\":[\"a\"],\"password\":\"x\"}")]
    public async Task LoginResistsInjectionPayloads(string payload)
    {
        var response = await client.PostAsync("/api/v1/auth/login", new StringContent(payload, Encoding.UTF8, "application/json"));

        Assert.Contains(response.StatusCode, new[] { HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized });
    }

    [Theory]
    [InlineData("/api/v1/users/me", "GET")]
    [InlineData("/api/v1/auth/logout", "POST")]
    [InlineData("/api/v1/quotes", "POST")]
    [InlineData("/api/v1/bookings", "POST")]
    public async Task ProtectedRoutesRequireAToken(string path, string method)
    {
        using var request = new HttpRequestMessage(new HttpMethod(method), path);
        if (method == "POST")
        {
            request.Content = JsonContent.Create(new { });
        }

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PublicProfileDoesNotRequireAToken()
    {
        using var response = await client.GetAsync(
            "/api/v1/users/8f6b8b48-f5d0-4f65-8fb0-2a7f0f6ab4b1/public");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PublicProfileRejectsOperatorInjectionAsAnUnknownId()
    {
        using var response = await client.GetAsync(
            "/api/v1/users/%24where%3A%22this.passwordHash%22/public");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("not-a-token")]
    [InlineData("eyJhbGciOiJub25lIn0.eyJzdWIiOiIxIn0.")]
    public async Task ForgedTokensAreRejected(string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/users/me");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RegisterIgnoresAnAttemptToChooseTheRole()
    {
        var email = $"{Guid.NewGuid():N}@virgo.test";
        var response = await client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            name = "Ana López",
            email,
            password = "Password-Segura-1",
            phone = "+524421234567",
            isVerified = true,
            providerId = "11111111-1111-4111-8111-111111111111"
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var text = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("passwordHash", text, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("11111111-1111", text, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ErrorsDoNotExposeInternals()
    {
        var response = await client.GetAsync("/api/v1/does-not-exist");
        var text = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.DoesNotContain("Exception", text, StringComparison.Ordinal);
        Assert.DoesNotContain("   at ", text, StringComparison.Ordinal);
    }
}
