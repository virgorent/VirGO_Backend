using VirGo.Domain.Auth;

namespace VirGo.Application.Auth;

public sealed record LoginCommand(string Email, string Password);

public sealed record LoginResult(
    string AccessToken,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt,
    User User);

public enum LoginFailure
{
    InvalidRequest,
    InvalidCredentials
}

public sealed record LoginOutcome(LoginResult? Result, LoginFailure? Failure)
{
    public bool IsSuccess => Result is not null;
}

public sealed record RegisterCommand(string Name, string Email, string Password, string Phone);

public enum RegisterFailure
{
    InvalidRequest,
    EmailAlreadyExists
}

public sealed record RegisterOutcome(User? User, RegisterFailure? Failure)
{
    public bool IsSuccess => User is not null;
}
