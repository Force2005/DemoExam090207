namespace DE.Forms.Models;

public sealed class AppUser
{
    public int UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Login { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string RoleName { get; set; } = string.Empty;

    public int FailedAttempts { get; set; }

    public bool IsBlocked { get; set; }

    public DateTime CreatedAt { get; set; }
}
