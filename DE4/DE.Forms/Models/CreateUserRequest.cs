namespace DE.Forms.Models;

public sealed class CreateUserRequest
{
    public string FullName { get; init; } = string.Empty;

    public string Login { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string RoleName { get; init; } = RoleNames.User;
}
