namespace DE.Forms.Infrastructure;

public static class DatabaseProvider
{
    public const string MySql = "MySql";

    public const string SqlServer = "SqlServer";

    public static bool IsSqlServer(string? provider)
    {
        return provider?.Trim().ToLowerInvariant() is "sqlserver" or "mssql" or "ms sql" or "ssms" or "smss";
    }

    public static bool IsMySql(string? provider)
    {
        return string.IsNullOrWhiteSpace(provider)
            || provider.Trim().Equals(MySql, StringComparison.OrdinalIgnoreCase)
            || provider.Trim().Equals("mysql", StringComparison.OrdinalIgnoreCase);
    }
}
