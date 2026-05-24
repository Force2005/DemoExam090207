namespace DE.Forms.Models;

public sealed class AppSettings
{
    public DatabaseSettings Database { get; set; } = new();
}
