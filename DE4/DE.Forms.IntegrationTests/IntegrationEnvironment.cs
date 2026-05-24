using DE.Forms.Models;
using System.Text.Json;

namespace DE.Forms.IntegrationTests;

internal static class IntegrationEnvironment
{
    private const string DatabasePrefix = "de_forms_it_";
    private static readonly Lazy<TestSettings> Settings = new(LoadSettings);

    public static bool IsMySqlEnabled()
    {
        return GetBool("DE_INTEGRATION_MYSQL", Settings.Value.MySql.Enabled);
    }

    public static bool IsSqlServerEnabled()
    {
        return GetBool("DE_INTEGRATION_SQLSERVER", Settings.Value.SqlServer.Enabled);
    }

    public static DatabaseSettings CreateMySqlSettings()
    {
        var mySql = Settings.Value.MySql;

        return new DatabaseSettings
        {
            Provider = "MySql",
            Server = GetString("DE_TEST_MYSQL_SERVER", mySql.Server),
            Port = GetUInt("DE_TEST_MYSQL_PORT", mySql.Port),
            DatabaseName = CreateDatabaseName("mysql"),
            UserId = GetString("DE_TEST_MYSQL_USER", mySql.UserId),
            Password = GetString("DE_TEST_MYSQL_PASSWORD", mySql.Password),
            ConnectionTimeout = GetUInt("DE_TEST_MYSQL_TIMEOUT", mySql.ConnectionTimeout)
        };
    }

    public static DatabaseSettings CreateSqlServerSettings()
    {
        var sqlServer = Settings.Value.SqlServer;
        var trustedConnection = GetBool("DE_TEST_SQLSERVER_TRUSTED", sqlServer.TrustedConnection);

        return new DatabaseSettings
        {
            Provider = "SqlServer",
            Server = GetString("DE_TEST_SQLSERVER_SERVER", sqlServer.Server),
            DatabaseName = CreateDatabaseName("sqlserver"),
            UserId = GetString("DE_TEST_SQLSERVER_USER", sqlServer.UserId),
            Password = GetString("DE_TEST_SQLSERVER_PASSWORD", sqlServer.Password),
            ConnectionTimeout = GetUInt("DE_TEST_SQLSERVER_TIMEOUT", sqlServer.ConnectionTimeout),
            TrustedConnection = trustedConnection,
            Encrypt = GetBool("DE_TEST_SQLSERVER_ENCRYPT", sqlServer.Encrypt),
            TrustServerCertificate = GetBool("DE_TEST_SQLSERVER_TRUST_CERT", sqlServer.TrustServerCertificate)
        };
    }

    public static void EnsureSafeTemporaryDatabaseName(string databaseName)
    {
        if (!databaseName.StartsWith(DatabasePrefix, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Refusing to drop database without '{DatabasePrefix}' prefix: {databaseName}");
        }
    }

    private static string CreateDatabaseName(string provider)
    {
        return $"{DatabasePrefix}{provider}_{Guid.NewGuid():N}";
    }

    private static TestSettings LoadSettings()
    {
        var settingsPath = Path.Combine(AppContext.BaseDirectory, "testsettings.json");
        if (!File.Exists(settingsPath))
        {
            settingsPath = Path.Combine(Environment.CurrentDirectory, "testsettings.json");
        }

        if (!File.Exists(settingsPath))
        {
            return new TestSettings();
        }

        var json = File.ReadAllText(settingsPath);
        return JsonSerializer.Deserialize<TestSettings>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new TestSettings();
    }

    private static string GetString(string variableName, string defaultValue)
    {
        return Environment.GetEnvironmentVariable(variableName) ?? defaultValue;
    }

    private static uint GetUInt(string variableName, uint defaultValue)
    {
        return uint.TryParse(Environment.GetEnvironmentVariable(variableName), out var value)
            ? value
            : defaultValue;
    }

    private static bool GetBool(string variableName, bool defaultValue)
    {
        var value = Environment.GetEnvironmentVariable(variableName);
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }

        return value.Equals("1", StringComparison.OrdinalIgnoreCase)
            || value.Equals("true", StringComparison.OrdinalIgnoreCase)
            || value.Equals("yes", StringComparison.OrdinalIgnoreCase);
    }
}
