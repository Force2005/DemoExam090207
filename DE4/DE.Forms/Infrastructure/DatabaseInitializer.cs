using DE.Forms.Services;
using MySqlConnector;

namespace DE.Forms.Infrastructure;

public sealed class DatabaseInitializer
{
    public const string UsersTableName = "users";

    private readonly MySqlConnectionFactory _connectionFactory;
    private readonly PasswordService _passwordService;

    public DatabaseInitializer(MySqlConnectionFactory connectionFactory, PasswordService passwordService)
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
        var sql = $"""
                   CREATE DATABASE IF NOT EXISTS {QuoteIdentifier(_connectionFactory.DatabaseName)}
                   CHARACTER SET utf8mb4
                   COLLATE utf8mb4_unicode_ci;
                   """;

        await using var connection = _connectionFactory.CreateServerConnection();
        await connection.OpenAsync();
        await using var command = new MySqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }

    private async Task EnsureUsersTableExistsAsync()
    {
        var sql = $"""
                   CREATE TABLE IF NOT EXISTS {QuoteIdentifier(UsersTableName)}
                   (
                       user_id INT UNSIGNED NOT NULL AUTO_INCREMENT,
                       full_name VARCHAR(200) NOT NULL,
                       login VARCHAR(100) NOT NULL,
                       password_hash VARCHAR(500) NOT NULL,
                       role_name VARCHAR(50) NOT NULL,
                       failed_attempts INT NOT NULL DEFAULT 0,
                       is_blocked TINYINT(1) NOT NULL DEFAULT 0,
                       created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                       PRIMARY KEY (user_id),
                       UNIQUE KEY uk_users_login (login)
                   )
                   ENGINE = InnoDB
                   DEFAULT CHARACTER SET = utf8mb4
                   COLLATE = utf8mb4_unicode_ci
                   COMMENT = 'Пользователи приложения';
                   """;

        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();
        await using (var command = new MySqlCommand(sql, connection))
        {
            await command.ExecuteNonQueryAsync();
        }

        await EnsureColumnExistsAsync(connection, "full_name", "VARCHAR(200) NOT NULL DEFAULT ''");
        await EnsureColumnExistsAsync(connection, "login", "VARCHAR(100) NOT NULL DEFAULT ''");
        await EnsureColumnExistsAsync(connection, "password_hash", "VARCHAR(500) NOT NULL DEFAULT ''");
        await EnsureColumnExistsAsync(connection, "role_name", "VARCHAR(50) NOT NULL DEFAULT 'Пользователь'");
        await EnsureColumnExistsAsync(connection, "failed_attempts", "INT NOT NULL DEFAULT 0");
        await EnsureColumnExistsAsync(connection, "is_blocked", "TINYINT(1) NOT NULL DEFAULT 0");
        await EnsureColumnExistsAsync(connection, "created_at", "DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP");
        await EnsureUniqueLoginIndexAsync(connection);
    }

    private async Task EnsureColumnExistsAsync(MySqlConnection connection, string columnName, string definition)
    {
        const string existsSql = """
                                 SELECT COUNT(*)
                                 FROM information_schema.COLUMNS
                                 WHERE TABLE_SCHEMA = @schema
                                   AND TABLE_NAME = @table
                                   AND COLUMN_NAME = @column;
                                 """;

        await using var existsCommand = new MySqlCommand(existsSql, connection);
        existsCommand.Parameters.AddWithValue("@schema", _connectionFactory.DatabaseName);
        existsCommand.Parameters.AddWithValue("@table", UsersTableName);
        existsCommand.Parameters.AddWithValue("@column", columnName);

        var count = Convert.ToInt32(await existsCommand.ExecuteScalarAsync());
        if (count > 0)
        {
            return;
        }

        var alterSql = $"ALTER TABLE {QuoteIdentifier(UsersTableName)} ADD COLUMN {QuoteIdentifier(columnName)} {definition};";
        await using var alterCommand = new MySqlCommand(alterSql, connection);
        await alterCommand.ExecuteNonQueryAsync();
    }

    private async Task EnsureUniqueLoginIndexAsync(MySqlConnection connection)
    {
        const string existsSql = """
                                 SELECT COUNT(*)
                                 FROM information_schema.STATISTICS
                                 WHERE TABLE_SCHEMA = @schema
                                   AND TABLE_NAME = @table
                                   AND INDEX_NAME = 'uk_users_login';
                                 """;

        await using var existsCommand = new MySqlCommand(existsSql, connection);
        existsCommand.Parameters.AddWithValue("@schema", _connectionFactory.DatabaseName);
        existsCommand.Parameters.AddWithValue("@table", UsersTableName);

        var count = Convert.ToInt32(await existsCommand.ExecuteScalarAsync());
        if (count > 0)
        {
            return;
        }

        var alterSql = $"ALTER TABLE {QuoteIdentifier(UsersTableName)} ADD UNIQUE KEY uk_users_login (login);";
        await using var alterCommand = new MySqlCommand(alterSql, connection);
        await alterCommand.ExecuteNonQueryAsync();
    }

    private async Task SeedUsersIfNeededAsync()
    {
        await using var connection = _connectionFactory.CreateApplicationConnection();
        await connection.OpenAsync();

        await using (var countCommand = new MySqlCommand($"SELECT COUNT(*) FROM {QuoteIdentifier(UsersTableName)};", connection))
        {
            var usersCount = Convert.ToInt32(await countCommand.ExecuteScalarAsync());
            if (usersCount > 0)
            {
                return;
            }
        }

        const string insertSql = """
                                 INSERT INTO users
                                     (full_name, login, password_hash, role_name, failed_attempts, is_blocked)
                                 VALUES
                                     (@adminName, @adminLogin, @adminPassword, @adminRole, 0, 0),
                                     (@userName, @userLogin, @userPassword, @userRole, 0, 0);
                                 """;

        await using var insertCommand = new MySqlCommand(insertSql, connection);
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

    private static string QuoteIdentifier(string identifier) => $"`{identifier.Replace("`", "``")}`";
}
