using VirGo.Application.Abstractions;

namespace VirGo.Application.Auth;

public sealed class DeleteAccountHandler(
    IUserRepository users,
    ISessionRepository sessions,
    IPasswordHasher passwordHasher,
    IClock clock)
{
    public async Task<bool> HandleAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await users.FindByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return false;
        }

        var now = clock.UtcNow;
        var anonymizedEmail = $"deleted+{user.Id}@invalid.virgo";
        var anonymizedPassword = passwordHasher.Hash(user, Guid.NewGuid().ToString("N"));

        // Revoke sessions before changing the account so a partial failure cannot leave an anonymized account authenticated.
        await sessions.DeleteAllAsync(user.Id, cancellationToken);

        var anonymized = await users.AnonymizeAsync(
            user.Id,
            anonymizedEmail,
            "0000000000000000",
            anonymizedPassword,
            now,
            cancellationToken);

        if (!anonymized)
        {
            return false;
        }

        return true;
    }
}
