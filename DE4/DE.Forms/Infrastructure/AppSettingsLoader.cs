using System.Text.Json;
using DE.Forms.Models;

namespace DE.Forms.Infrastructure;

public static class AppSettingsLoader
{
    public static AppSettings Load()
    {
        var settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (!File.Exists(settingsPath))
        {
            return new AppSettings();
        }

        var json = File.ReadAllText(settingsPath);
        var settings = JsonSerializer.Deserialize<AppSettings>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return settings ?? new AppSettings();
    }
}
