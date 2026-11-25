using VirGo.Application.Abstractions;
using VirGo.Domain.Auth;

namespace VirGo.Application.Auth;

public sealed class UpdateProfileHandler(
    IUserRepository users,
    IClock clock)
{
    public async Task<User?> HandleAsync(
        string userId,
        UpdateProfileCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await users.FindByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return null;
        }

        if (command.Name is not null)
        {
            user.Name = command.Name;
        }

        if (command.Phone is not null)
        {
            user.Phone = command.Phone;
        }

        if (command.City is not null)
        {
            user.City = string.IsNullOrWhiteSpace(command.City) ? null : command.City;
        }

        if (command.DefaultGuests is not null)
        {
            user.DefaultGuests = command.DefaultGuests.Value;
        }

        user.UpdatedAt = clock.UtcNow;
        await users.UpdateAsync(user, cancellationToken);
        return user;
    }
}
