namespace DE.Forms.Models;

public sealed class AuthenticationResult
{
    private AuthenticationResult(AuthenticationStatus status, string message, AppUser? user = null)
    {
        Status = status;
        Message = message;
        User = user;
    }

    public AuthenticationStatus Status { get; }

    public string Message { get; }

    public AppUser? User { get; }

    public bool IsSuccess => Status == AuthenticationStatus.Success;

    public bool IsBlocked => Status == AuthenticationStatus.Blocked;

    public static AuthenticationResult Success(AppUser user) =>
        new(AuthenticationStatus.Success, "Вы успешно авторизовались", user);

    public static AuthenticationResult InvalidCredentials() =>
        new(AuthenticationStatus.InvalidCredentials, "Вы ввели неверный логин или пароль. Пожалуйста проверьте ещё раз введенные данные");

    public static AuthenticationResult Blocked(AppUser? user = null) =>
        new(AuthenticationStatus.Blocked, "Вы заблокированы. Обратитесь к администратору", user);
}
