using VirGo.Application.Abstractions;

namespace VirGo.Application.Auth;

public sealed class ChangePasswordHandler(
    IUserRepository users,
    IPasswordHasher passwordHasher,
    IClock clock)
{
    public async Task<bool> HandleAsync(
        string userId,
        ChangePasswordCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await users.FindByIdAsync(userId, cancellationToken);
        if (user is null || !passwordHasher.Verify(user, user.PasswordHash, command.CurrentPassword))
        {
            return false;
        }

        user.PasswordHash = passwordHasher.Hash(user, command.NewPassword);
        user.UpdatedAt = clock.UtcNow;
        await users.UpdateAsync(user, cancellationToken);
        return true;
    }
}
