using DE.Forms.Infrastructure;
using DE.Forms.Models;
using DE.Forms.Services;
using Microsoft.Data.SqlClient;

namespace DE.Forms.IntegrationTests;

public sealed class SqlServerUserWorkflowTests
{
    [SqlServerIntegrationFact]
    public async Task UserWorkflow_RunsAgainstSqlServer()
    {
        var settings = IntegrationEnvironment.CreateSqlServerSettings();
        var passwordService = new PasswordService();
        var connectionFactory = new SqlServerConnectionFactory(settings);
        var initializer = new SqlServerDatabaseInitializer(connectionFactory, passwordService);
        var repository = new SqlServerUserRepository(connectionFactory);

        try
        {
            await DropDatabaseIfExistsAsync(settings);
            await initializer.InitializeAsync();

            await UserWorkflowAssertions.RunAsync(repository);
        }
        finally
        {
            await DropDatabaseIfExistsAsync(settings);
        }
    }

    private static async Task DropDatabaseIfExistsAsync(DatabaseSettings settings)
    {
        IntegrationEnvironment.EnsureSafeTemporaryDatabaseName(settings.DatabaseName);
        SqlConnection.ClearAllPools();

        var builder = new SqlConnectionStringBuilder
        {
            DataSource = settings.Server,
            InitialCatalog = "master",
            IntegratedSecurity = settings.TrustedConnection,
            Encrypt = settings.Encrypt,
            TrustServerCertificate = settings.TrustServerCertificate,
            ConnectTimeout = (int)settings.ConnectionTimeout
        };

        if (!settings.TrustedConnection)
        {
            builder.UserID = settings.UserId;
            builder.Password = settings.Password;
        }

        var databaseName = QuoteIdentifier(settings.DatabaseName);
        var sql = $"""
                   IF DB_ID(N'{settings.DatabaseName.Replace("'", "''")}') IS NOT NULL
                   BEGIN
                       ALTER DATABASE {databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                       DROP DATABASE {databaseName};
                   END;
                   """;

        await using var connection = new SqlConnection(builder.ConnectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }

    private static string QuoteIdentifier(string identifier) => $"[{identifier.Replace("]", "]]")}]";
}
