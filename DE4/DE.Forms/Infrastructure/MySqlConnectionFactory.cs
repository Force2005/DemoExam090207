using DE.Forms.Models;
using MySqlConnector;

namespace DE.Forms.Infrastructure;

public sealed class MySqlConnectionFactory
{
    private readonly DatabaseSettings _settings;

    public MySqlConnectionFactory(DatabaseSettings settings)
    {
        _settings = settings;
    }

    public string DatabaseName => _settings.DatabaseName;

    public MySqlConnection CreateServerConnection() => new(BuildConnectionString(null));

    public MySqlConnection CreateApplicationConnection() => new(BuildConnectionString(_settings.DatabaseName));

    private string BuildConnectionString(string? databaseName)
    {
        var builder = new MySqlConnectionStringBuilder
        {
            Server = _settings.Server,
            Port = _settings.Port,
            UserID = _settings.UserId,
            Password = _settings.Password,
            CharacterSet = "utf8mb4",
            ConnectionTimeout = _settings.ConnectionTimeout,
            SslMode = MySqlSslMode.None
        };

        if (!string.IsNullOrWhiteSpace(databaseName))
        {
            builder.Database = databaseName;
        }

        return builder.ConnectionString;
    }
}
