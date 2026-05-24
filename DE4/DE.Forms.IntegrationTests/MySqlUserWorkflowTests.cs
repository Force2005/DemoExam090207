using DE.Forms.Infrastructure;
using DE.Forms.Services;
using MySqlConnector;
using Xunit;

namespace DE.Forms.IntegrationTests;

public sealed class MySqlUserWorkflowTests
{
    [MySqlIntegrationFact]
    public async Task UserWorkflow_RunsAgainstMySql()
    {
        var settings = IntegrationEnvironment.CreateMySqlSettings();
        var passwordService = new PasswordService();
        var connectionFactory = new MySqlConnectionFactory(settings);
        var initializer = new DatabaseInitializer(connectionFactory, passwordService);
        var repository = new UserRepository(connectionFactory);

        try
        {
            await DropDatabaseIfExistsAsync(settings.Server, settings.Port, settings.UserId, settings.Password, settings.ConnectionTimeout, settings.DatabaseName);
            await initializer.InitializeAsync();

            await UserWorkflowAssertions.RunAsync(repository);
        }
        finally
        {
            await DropDatabaseIfExistsAsync(settings.Server, settings.Port, settings.UserId, settings.Password, settings.ConnectionTimeout, settings.DatabaseName);
        }
    }

    private static async Task DropDatabaseIfExistsAsync(
        string server,
        uint port,
        string userId,
        string password,
        uint connectionTimeout,
        string databaseName)
    {
        IntegrationEnvironment.EnsureSafeTemporaryDatabaseName(databaseName);
        MySqlConnection.ClearAllPools();

        var builder = new MySqlConnectionStringBuilder
        {
            Server = server,
            Port = port,
            UserID = userId,
            Password = password,
            CharacterSet = "utf8mb4",
            ConnectionTimeout = connectionTimeout,
            SslMode = MySqlSslMode.None
        };

        await using var connection = new MySqlConnection(builder.ConnectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand($"DROP DATABASE IF EXISTS `{databaseName.Replace("`", "``")}`;", connection);
        await command.ExecuteNonQueryAsync();
    }
}
