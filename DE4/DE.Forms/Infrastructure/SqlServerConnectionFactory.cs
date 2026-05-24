using DE.Forms.Models;
using Microsoft.Data.SqlClient;

namespace DE.Forms.Infrastructure;

public sealed class SqlServerConnectionFactory
{
    private readonly DatabaseSettings _settings;

    public SqlServerConnectionFactory(DatabaseSettings settings)
    {
        _settings = settings;
    }

    public string DatabaseName => _settings.DatabaseName;

    public SqlConnection CreateMasterConnection() => new(BuildConnectionString("master"));

    public SqlConnection CreateApplicationConnection() => new(BuildConnectionString(_settings.DatabaseName));

    private string BuildConnectionString(string databaseName)
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = _settings.Server,
            InitialCatalog = databaseName,
            IntegratedSecurity = _settings.TrustedConnection,
            Encrypt = _settings.Encrypt,
            TrustServerCertificate = _settings.TrustServerCertificate,
            ConnectTimeout = (int)_settings.ConnectionTimeout,
            MultipleActiveResultSets = false
        };

        if (!_settings.TrustedConnection)
        {
            builder.UserID = _settings.UserId;
            builder.Password = _settings.Password;
        }

        return builder.ConnectionString;
    }
}
