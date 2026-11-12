using Microsoft.Extensions.Configuration;
using VirGo.Application.Abstractions;
using VirGo.Application.Auth;
using VirGo.Domain.Auth;
using VirGo.Infrastructure.Auth;

namespace VirGo.UnitTests;

public sealed class LoginHandlerTests
{
    [Fact]
    public async Task ExpiredLockoutResetsBeforeAnewFailedAttempt()
    {
        var clock = new TestClock(new DateTime(2026, 11, 11, 12, 0, 0, DateTimeKind.Utc));
        var passwordHasher = new IdentityPasswordHasher();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Authentication:DemoEmail"] = "demo@virgo.mx",
                ["Authentication:DemoPassword"] = "correct-password"
            })
            .Build();
        var users = new InMemoryUserRepository(configuration, passwordHasher, clock);
        var handler = new LoginHandler(
            users,
            new InMemorySessionRepository(),
            passwordHasher,
            new TestTokenService(),
            new TestRefreshTokenGenerator(),
            clock);

        for (var attempt = 0; attempt < 5; attempt++)
        {
            var failed = await handler.HandleAsync(new LoginCommand("demo@virgo.mx", "wrong-password"));
            Assert.Equal(LoginFailure.InvalidCredentials, failed.Failure);
        }

        clock.UtcNow = clock.UtcNow.AddMinutes(16);
        var failedAfterExpiry = await handler.HandleAsync(new LoginCommand("demo@virgo.mx", "wrong-password"));

        Assert.Equal(LoginFailure.InvalidCredentials, failedAfterExpiry.Failure);
        var user = await users.FindByEmailAsync("demo@virgo.mx");
        Assert.Equal(1, user!.FailedLoginCount);
        Assert.Null(user.LockoutEndsAt);
    }

    private sealed class TestClock(DateTime utcNow) : IClock
    {
        public DateTime UtcNow { get; set; } = utcNow;
    }

    private sealed class TestTokenService : ITokenService
    {
        public string CreateAccessToken(User user, Session session, DateTime utcNow) => "test-access-token";
    }

    private sealed class TestRefreshTokenGenerator : IRefreshTokenGenerator
    {
        public string Create() => "test-refresh-token";

        public string Hash(string token) => $"hash:{token}";
    }
}
