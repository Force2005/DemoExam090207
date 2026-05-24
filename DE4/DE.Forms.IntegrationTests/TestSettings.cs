namespace DE.Forms.IntegrationTests;

internal sealed class TestSettings
{
    public ProviderTestSettings MySql { get; set; } = new()
    {
        Server = "localhost",
        Port = 3306,
        UserId = "root"
    };

    public ProviderTestSettings SqlServer { get; set; } = new()
    {
        Server = @"(localdb)\MSSQLLocalDB",
        TrustedConnection = true,
        TrustServerCertificate = true
    };
}

internal sealed class ProviderTestSettings
{
    public bool Enabled { get; set; }

    public string Server { get; set; } = string.Empty;

    public uint Port { get; set; } = 3306;

    public string UserId { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public uint ConnectionTimeout { get; set; } = 15;

    public bool TrustedConnection { get; set; } = true;

    public bool Encrypt { get; set; }

    public bool TrustServerCertificate { get; set; } = true;
}
