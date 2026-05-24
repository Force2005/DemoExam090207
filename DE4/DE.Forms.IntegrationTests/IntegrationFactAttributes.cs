using Xunit;

namespace DE.Forms.IntegrationTests;

public sealed class MySqlIntegrationFactAttribute : FactAttribute
{
    public MySqlIntegrationFactAttribute()
    {
        if (!IntegrationEnvironment.IsMySqlEnabled())
        {
            Skip = "Enable MySQL integration tests in testsettings.json or set DE_INTEGRATION_MYSQL=1.";
        }
    }
}

public sealed class SqlServerIntegrationFactAttribute : FactAttribute
{
    public SqlServerIntegrationFactAttribute()
    {
        if (!IntegrationEnvironment.IsSqlServerEnabled())
        {
            Skip = "Enable SQL Server integration tests in testsettings.json or set DE_INTEGRATION_SQLSERVER=1.";
        }
    }
}
