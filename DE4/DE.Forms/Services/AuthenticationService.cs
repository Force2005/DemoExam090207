using DE.Forms.Infrastructure;
using DE.Forms.Models;

namespace DE.Forms.Services;

public sealed class AuthenticationService
{
    public const int MaxFailedAttempts = 3;

    private readonly IUserRepository _userRepository;
    private readonly PasswordService _passwordService;

    public AuthenticationService(IUserRepository userRepository, PasswordService passwordService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
    }

    public async Task<AuthenticationResult> AuthenticateAsync(string login, string password)
    {
        var normalizedLogin = login.Trim();
        var user = await _userRepository.GetByLoginAsync(normalizedLogin);

        if (user is null)
        {
            return AuthenticationResult.InvalidCredentials();
        }

        if (user.IsBlocked)
        {
            return AuthenticationResult.Blocked(user);
        }

        if (!_passwordService.VerifyPassword(password, user.PasswordHash))
        {
            return await RegisterFailedAttemptAsync(user);
        }

        await _userRepository.ResetFailedAttemptsAsync(user.UserId);
        user.FailedAttempts = 0;
        user.IsBlocked = false;
        return AuthenticationResult.Success(user);
    }

    public async Task<AuthenticationResult> RegisterFailedCaptchaAttemptAsync(string login)
    {
        var normalizedLogin = login.Trim();
        var user = await _userRepository.GetByLoginAsync(normalizedLogin);

        if (user is null)
        {
            return AuthenticationResult.InvalidCredentials();
        }

        if (user.IsBlocked)
        {
            return AuthenticationResult.Blocked(user);
        }

        return await RegisterFailedAttemptAsync(user);
    }

    private async Task<AuthenticationResult> RegisterFailedAttemptAsync(AppUser user)
    {
        var updatedUser = await _userRepository.IncrementFailedAttemptsAsync(user.UserId, MaxFailedAttempts);
        if (updatedUser?.IsBlocked == true)
        {
            return AuthenticationResult.Blocked(updatedUser);
        }

        return AuthenticationResult.InvalidCredentials();
    }
}
