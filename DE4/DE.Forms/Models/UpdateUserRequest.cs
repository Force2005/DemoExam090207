namespace DE.Forms.Models;

public sealed class UpdateUserRequest
{
    public int UserId { get; init; }

    public string FullName { get; init; } = string.Empty;

    public string Login { get; init; } = string.Empty;

    public string NewPassword { get; init; } = string.Empty;

    public string RoleName { get; init; } = RoleNames.User;

    public bool IsBlocked { get; init; }
}
