using DE.Forms.Services;
using Microsoft.Data.SqlClient;

namespace DE.Forms.Infrastructure;

public sealed class SqlServerDatabaseInitializer
{
    private readonly SqlServerConnectionFactory _connectionFactory;
    private readonly PasswordService _passwordService;

    public SqlServerDatabaseInitializer(SqlServerConnectionFactory connectionFactory, PasswordService passwordService)
    {
        _connectionFactory = connectionFactory;
        _passwordService = passwordService;
    }

    public async Task InitializeAsync()
    {
        await EnsureDatabaseExistsAsync();
        await EnsureUsersTableExistsAsync();
        await SeedUsersIfNeededAsync();
    }

    private async Task EnsureDatabaseExistsAsync()
    {
        const string sql = """
                           IF DB_ID(@databaseName) IS NULL
                           BEGIN
                               DECLARE @sql nvarchar(max) = N'CREATE DATABASE ' + QUOTENAME(@databaseName);
                               EXEC (@sql);
                           END;
                           """;

        await using var connection = _connectionFactory.CreateMasterConnection();
        await connection.OpenAsync();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@databaseName", _connectionFactory.DatabaseName);
        await command.ExecuteNonQueryAsync();
    }

    private async Task EnsureUsersTableExistsAsync()
    {
        const string createSql = """
                                 IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
                                 BEGIN
                                     CREATE TABLE dbo.Users
                                     (
                                         UserId int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
                                         FullName nvarchar(200) NOT NULL,
                                         Login nvarchar(100) NOT NULL,
                                         PasswordHash nvarchar(500) NOT NULL,
                                         RoleName nvarchar(50) NOT NULL,
                                         FailedAttempts int NOT NULL CONSTRAINT DF_Users_FailedAttempts DEFAULT (0),
                                         IsBlocked bit NOT NULL CONSTRAINT DF_Users_IsBlocked DEFAULT (0),
                                         CreatedAt datetime2 NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT (SYSDATETIME())
                                     );
                                 END;
                                 """;

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using (var createCommand = new SqlCommand(createSql, connection))
        {
            await createCommand.ExecuteNonQueryAsync();
        }

        await EnsureColumnExistsAsync(connection, "FullName", "nvarchar(200) NOT NULL CONSTRAINT DF_Users_FullName DEFAULT (N'')");
        await EnsureColumnExistsAsync(connection, "Login", "nvarchar(100) NOT NULL CONSTRAINT DF_Users_Login DEFAULT (N'')");
        await EnsureColumnExistsAsync(connection, "PasswordHash", "nvarchar(500) NOT NULL CONSTRAINT DF_Users_PasswordHash DEFAULT (N'')");
        await EnsureColumnExistsAsync(connection, "RoleName", "nvarchar(50) NOT NULL CONSTRAINT DF_Users_RoleName DEFAULT (N'Пользователь')");
        await EnsureColumnExistsAsync(connection, "FailedAttempts", "int NOT NULL CONSTRAINT DF_Users_FailedAttempts DEFAULT (0)");
        await EnsureColumnExistsAsync(connection, "IsBlocked", "bit NOT NULL CONSTRAINT DF_Users_IsBlocked DEFAULT (0)");
        await EnsureColumnExistsAsync(connection, "CreatedAt", "datetime2 NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT (SYSDATETIME())");
        await EnsureUniqueLoginIndexAsync(connection);
    }

    private static async Task EnsureColumnExistsAsync(SqlConnection connection, string columnName, string definition)
    {
        const string existsSql = """
                                 SELECT COUNT(1)
                                 FROM sys.columns
                                 WHERE object_id = OBJECT_ID(N'dbo.Users')
                                   AND name = @columnName;
                                 """;

        await using var existsCommand = new SqlCommand(existsSql, connection);
        existsCommand.Parameters.AddWithValue("@columnName", columnName);

        var count = Convert.ToInt32(await existsCommand.ExecuteScalarAsync());
        if (count > 0)
        {
            return;
        }

        var alterSql = $"ALTER TABLE dbo.Users ADD {QuoteIdentifier(columnName)} {definition};";
        await using var alterCommand = new SqlCommand(alterSql, connection);
        await alterCommand.ExecuteNonQueryAsync();
    }

    private static async Task EnsureUniqueLoginIndexAsync(SqlConnection connection)
    {
        const string existsSql = """
                                 SELECT COUNT(1)
                                 FROM sys.indexes
                                 WHERE object_id = OBJECT_ID(N'dbo.Users')
                                   AND name = N'UX_Users_Login';
                                 """;

        await using var existsCommand = new SqlCommand(existsSql, connection);
        var count = Convert.ToInt32(await existsCommand.ExecuteScalarAsync());
        if (count > 0)
        {
            return;
        }

        await using var createIndexCommand = new SqlCommand("CREATE UNIQUE INDEX UX_Users_Login ON dbo.Users(Login);", connection);
        await createIndexCommand.ExecuteNonQueryAsync();
    }

    private async Task SeedUsersIfNeededAsync()
    {
        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();

        await using (var countCommand = new SqlCommand("SELECT COUNT(1) FROM dbo.Users;", connection))
        {
            var usersCount = Convert.ToInt32(await countCommand.ExecuteScalarAsync());
            if (usersCount > 0)
            {
                return;
            }
        }

        const string insertSql = """
                                 INSERT INTO dbo.Users (FullName, Login, PasswordHash, RoleName, FailedAttempts, IsBlocked)
                                 VALUES
                                     (@adminName, @adminLogin, @adminPassword, @adminRole, 0, 0),
                                     (@userName, @userLogin, @userPassword, @userRole, 0, 0);
                                 """;

        await using var insertCommand = new SqlCommand(insertSql, connection);
        insertCommand.Parameters.AddWithValue("@adminName", "Системный администратор");
        insertCommand.Parameters.AddWithValue("@adminLogin", "admin");
        insertCommand.Parameters.AddWithValue("@adminPassword", _passwordService.HashPassword("admin"));
        insertCommand.Parameters.AddWithValue("@adminRole", "Администратор");
        insertCommand.Parameters.AddWithValue("@userName", "Тестовый пользователь");
        insertCommand.Parameters.AddWithValue("@userLogin", "user");
        insertCommand.Parameters.AddWithValue("@userPassword", _passwordService.HashPassword("user"));
        insertCommand.Parameters.AddWithValue("@userRole", "Пользователь");
        await insertCommand.ExecuteNonQueryAsync();
    }

    private static string QuoteIdentifier(string identifier) => $"[{identifier.Replace("]", "]]")}]";
}
