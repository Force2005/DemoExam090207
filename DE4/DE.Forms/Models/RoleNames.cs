namespace DE.Forms.Models;

public static class RoleNames
{
    public const string Administrator = "Администратор";

    public const string User = "Пользователь";

    public static IReadOnlyList<string> All { get; } = new[] { Administrator, User };
}
