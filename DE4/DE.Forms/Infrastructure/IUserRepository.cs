using DE.Forms.Models;

namespace DE.Forms.Infrastructure;

public interface IUserRepository
{
    Task<AppUser?> GetByLoginAsync(string login);

    Task<AppUser?> GetByIdAsync(int userId);

    Task<IReadOnlyList<AppUser>> SearchAsync(string? searchText);

    Task<bool> LoginExistsAsync(string login, int? excludingUserId = null);

    Task<int> CreateAsync(AppUser user);

    Task UpdateAsync(AppUser user);

    Task ResetFailedAttemptsAsync(int userId);

    Task<AppUser?> IncrementFailedAttemptsAsync(int userId, int maxFailedAttempts);

    Task SetBlockedStatusAsync(int userId, bool isBlocked);
}
