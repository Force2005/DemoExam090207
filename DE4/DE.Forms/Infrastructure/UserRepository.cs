using DE.Forms.Models;
using MySqlConnector;

namespace DE.Forms.Infrastructure;

public sealed class UserRepository : IUserRepository
{
    private readonly MySqlConnectionFactory _connectionFactory;

    public UserRepository(MySqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<AppUser?> GetByLoginAsync(string login)
    {
        const string sql = """
                           SELECT user_id, full_name, login, password_hash, role_name, failed_attempts, is_blocked, created_at
                           FROM users
                           WHERE login = @login;
                           """;

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@login", login);

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? ReadUser(reader) : null;
    }

    public async Task<AppUser?> GetByIdAsync(int userId)
    {
        const string sql = """
                           SELECT user_id, full_name, login, password_hash, role_name, failed_attempts, is_blocked, created_at
                           FROM users
                           WHERE user_id = @userId;
                           """;

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@userId", userId);

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? ReadUser(reader) : null;
    }

    public async Task<IReadOnlyList<AppUser>> SearchAsync(string? searchText)
    {
        const string sql = """
                           SELECT user_id, full_name, login, password_hash, role_name, failed_attempts, is_blocked, created_at
                           FROM users
                           WHERE @searchText = '' OR login LIKE @pattern OR full_name LIKE @pattern
                           ORDER BY login;
                           """;

        var normalizedSearchText = searchText?.Trim() ?? string.Empty;
        var users = new List<AppUser>();

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@searchText", normalizedSearchText);
        command.Parameters.AddWithValue("@pattern", $"%{normalizedSearchText}%");

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            users.Add(ReadUser(reader));
        }

        return users;
    }

    public async Task<bool> LoginExistsAsync(string login, int? excludingUserId = null)
    {
        const string sql = """
                           SELECT COUNT(*)
                           FROM users
                           WHERE login = @login
                             AND (@excludingUserId IS NULL OR user_id <> @excludingUserId);
                           """;

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@login", login);
        command.Parameters.AddWithValue("@excludingUserId", excludingUserId.HasValue ? excludingUserId.Value : DBNull.Value);

        return Convert.ToInt32(await command.ExecuteScalarAsync()) > 0;
    }

    public async Task<int> CreateAsync(AppUser user)
    {
        const string sql = """
                           INSERT INTO users (full_name, login, password_hash, role_name, failed_attempts, is_blocked)
                           VALUES (@fullName, @login, @passwordHash, @roleName, @failedAttempts, @isBlocked);
                           SELECT LAST_INSERT_ID();
                           """;

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using var command = CreateUserCommand(sql, connection, user);

        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    public async Task UpdateAsync(AppUser user)
    {
        const string sql = """
                           UPDATE users
                           SET full_name = @fullName,
                               login = @login,
                               password_hash = @passwordHash,
                               role_name = @roleName,
                               failed_attempts = @failedAttempts,
                               is_blocked = @isBlocked
                           WHERE user_id = @userId;
                           """;

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using var command = CreateUserCommand(sql, connection, user);
        command.Parameters.AddWithValue("@userId", user.UserId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task ResetFailedAttemptsAsync(int userId)
    {
        const string sql = """
                           UPDATE users
                           SET failed_attempts = 0,
                               is_blocked = 0
                           WHERE user_id = @userId;
                           """;

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@userId", userId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task<AppUser?> IncrementFailedAttemptsAsync(int userId, int maxFailedAttempts)
    {
        const string sql = """
                           UPDATE users
                           SET failed_attempts = failed_attempts + 1,
                               is_blocked = CASE
                                   WHEN failed_attempts + 1 >= @maxFailedAttempts THEN 1
                                   ELSE is_blocked
                               END
                           WHERE user_id = @userId;
                           """;

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@userId", userId);
        command.Parameters.AddWithValue("@maxFailedAttempts", maxFailedAttempts);
        await command.ExecuteNonQueryAsync();

        return await GetByIdAsync(userId);
    }

    public async Task SetBlockedStatusAsync(int userId, bool isBlocked)
    {
        const string sql = """
                           UPDATE users
                           SET is_blocked = @isBlocked,
                               failed_attempts = CASE WHEN @isBlocked = 0 THEN 0 ELSE failed_attempts END
                           WHERE user_id = @userId;
                           """;

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@userId", userId);
        command.Parameters.AddWithValue("@isBlocked", isBlocked);
        await command.ExecuteNonQueryAsync();
    }

    private static MySqlCommand CreateUserCommand(string sql, MySqlConnection connection, AppUser user)
    {
        var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@fullName", user.FullName);
        command.Parameters.AddWithValue("@login", user.Login);
        command.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
        command.Parameters.AddWithValue("@roleName", user.RoleName);
        command.Parameters.AddWithValue("@failedAttempts", user.FailedAttempts);
        command.Parameters.AddWithValue("@isBlocked", user.IsBlocked);
        return command;
    }

    private static AppUser ReadUser(MySqlDataReader reader)
    {
        var userIdOrdinal = reader.GetOrdinal("user_id");
        var fullNameOrdinal = reader.GetOrdinal("full_name");
        var loginOrdinal = reader.GetOrdinal("login");
        var passwordHashOrdinal = reader.GetOrdinal("password_hash");
        var roleNameOrdinal = reader.GetOrdinal("role_name");
        var failedAttemptsOrdinal = reader.GetOrdinal("failed_attempts");
        var isBlockedOrdinal = reader.GetOrdinal("is_blocked");
        var createdAtOrdinal = reader.GetOrdinal("created_at");

        return new AppUser
        {
            UserId = reader.GetInt32(userIdOrdinal),
            FullName = reader.GetString(fullNameOrdinal),
            Login = reader.GetString(loginOrdinal),
            PasswordHash = reader.GetString(passwordHashOrdinal),
            RoleName = reader.GetString(roleNameOrdinal),
            FailedAttempts = reader.GetInt32(failedAttemptsOrdinal),
            IsBlocked = reader.GetBoolean(isBlockedOrdinal),
            CreatedAt = reader.GetDateTime(createdAtOrdinal)
        };
    }
}
