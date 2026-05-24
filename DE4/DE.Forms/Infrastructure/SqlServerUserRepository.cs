using DE.Forms.Models;
using Microsoft.Data.SqlClient;

namespace DE.Forms.Infrastructure;

public sealed class SqlServerUserRepository : IUserRepository
{
    private readonly SqlServerConnectionFactory _connectionFactory;

    public SqlServerUserRepository(SqlServerConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<AppUser?> GetByLoginAsync(string login)
    {
        const string sql = """
                           SELECT UserId, FullName, Login, PasswordHash, RoleName, FailedAttempts, IsBlocked, CreatedAt
                           FROM dbo.Users
                           WHERE Login = @login;
                           """;

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@login", login);

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? ReadUser(reader) : null;
    }

    public async Task<AppUser?> GetByIdAsync(int userId)
    {
        const string sql = """
                           SELECT UserId, FullName, Login, PasswordHash, RoleName, FailedAttempts, IsBlocked, CreatedAt
                           FROM dbo.Users
                           WHERE UserId = @userId;
                           """;

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@userId", userId);

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? ReadUser(reader) : null;
    }

    public async Task<IReadOnlyList<AppUser>> SearchAsync(string? searchText)
    {
        const string sql = """
                           SELECT UserId, FullName, Login, PasswordHash, RoleName, FailedAttempts, IsBlocked, CreatedAt
                           FROM dbo.Users
                           WHERE @searchText = N'' OR Login LIKE @pattern OR FullName LIKE @pattern
                           ORDER BY Login;
                           """;

        var normalizedSearchText = searchText?.Trim() ?? string.Empty;
        var users = new List<AppUser>();

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using var command = new SqlCommand(sql, connection);
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
                           SELECT COUNT(1)
                           FROM dbo.Users
                           WHERE Login = @login
                             AND (@excludingUserId IS NULL OR UserId <> @excludingUserId);
                           """;

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@login", login);
        command.Parameters.AddWithValue("@excludingUserId", excludingUserId.HasValue ? excludingUserId.Value : DBNull.Value);

        return Convert.ToInt32(await command.ExecuteScalarAsync()) > 0;
    }

    public async Task<int> CreateAsync(AppUser user)
    {
        const string sql = """
                           INSERT INTO dbo.Users (FullName, Login, PasswordHash, RoleName, FailedAttempts, IsBlocked)
                           OUTPUT INSERTED.UserId
                           VALUES (@fullName, @login, @passwordHash, @roleName, @failedAttempts, @isBlocked);
                           """;

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using var command = CreateUserCommand(sql, connection, user);

        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    public async Task UpdateAsync(AppUser user)
    {
        const string sql = """
                           UPDATE dbo.Users
                           SET FullName = @fullName,
                               Login = @login,
                               PasswordHash = @passwordHash,
                               RoleName = @roleName,
                               FailedAttempts = @failedAttempts,
                               IsBlocked = @isBlocked
                           WHERE UserId = @userId;
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
                           UPDATE dbo.Users
                           SET FailedAttempts = 0,
                               IsBlocked = 0
                           WHERE UserId = @userId;
                           """;

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@userId", userId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task<AppUser?> IncrementFailedAttemptsAsync(int userId, int maxFailedAttempts)
    {
        const string sql = """
                           UPDATE dbo.Users
                           SET FailedAttempts = FailedAttempts + 1,
                               IsBlocked = CASE
                                   WHEN FailedAttempts + 1 >= @maxFailedAttempts THEN 1
                                   ELSE IsBlocked
                               END
                           WHERE UserId = @userId;
                           """;

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@userId", userId);
        command.Parameters.AddWithValue("@maxFailedAttempts", maxFailedAttempts);
        await command.ExecuteNonQueryAsync();

        return await GetByIdAsync(userId);
    }

    public async Task SetBlockedStatusAsync(int userId, bool isBlocked)
    {
        const string sql = """
                           UPDATE dbo.Users
                           SET IsBlocked = @isBlocked,
                               FailedAttempts = CASE WHEN @isBlocked = 0 THEN 0 ELSE FailedAttempts END
                           WHERE UserId = @userId;
                           """;

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@userId", userId);
        command.Parameters.AddWithValue("@isBlocked", isBlocked);
        await command.ExecuteNonQueryAsync();
    }

    private static SqlCommand CreateUserCommand(string sql, SqlConnection connection, AppUser user)
    {
        var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@fullName", user.FullName);
        command.Parameters.AddWithValue("@login", user.Login);
        command.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
        command.Parameters.AddWithValue("@roleName", user.RoleName);
        command.Parameters.AddWithValue("@failedAttempts", user.FailedAttempts);
        command.Parameters.AddWithValue("@isBlocked", user.IsBlocked);
        return command;
    }

    private static AppUser ReadUser(SqlDataReader reader)
    {
        return new AppUser
        {
            UserId = reader.GetInt32(0),
            FullName = reader.GetString(1),
            Login = reader.GetString(2),
            PasswordHash = reader.GetString(3),
            RoleName = reader.GetString(4),
            FailedAttempts = reader.GetInt32(5),
            IsBlocked = reader.GetBoolean(6),
            CreatedAt = reader.GetDateTime(7)
        };
    }
}
