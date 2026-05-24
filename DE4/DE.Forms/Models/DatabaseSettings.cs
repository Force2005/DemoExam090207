namespace DE.Forms.Models;

public sealed class DatabaseSettings
{
    public string Provider { get; set; } = "MySql";

    public string Server { get; set; } = "localhost";

    public uint Port { get; set; } = 3306;

    public string DatabaseName { get; set; } = "demo_090207_m1_m2";

    public string UserId { get; set; } = "root";

    public string Password { get; set; } = string.Empty;

    public uint ConnectionTimeout { get; set; } = 15;

    public bool TrustedConnection { get; set; } = true;

    public bool Encrypt { get; set; }

    public bool TrustServerCertificate { get; set; } = true;
}
