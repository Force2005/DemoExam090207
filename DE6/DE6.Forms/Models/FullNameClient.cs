using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DE6.Forms.Models;

internal static class FullNameClient
{
    private static readonly Uri[] ApiEndpoints =
    [
        new("http://localhost:4444/TransferSimulator/fullName"),
        new("http://prb.sylas.ru/TransferSimulator/fullName")
    ];

    private static readonly HttpClient HttpClient = new(new HttpClientHandler { UseProxy = false })
    {
        Timeout = TimeSpan.FromSeconds(8)
    };

    public static async Task<string> GetFullNameAsync(CancellationToken cancellationToken = default)
    {
        List<string> errors = [];

        foreach (Uri endpoint in ApiEndpoints)
        {
            try
            {
                using HttpResponseMessage response = await HttpClient.GetAsync(endpoint, cancellationToken);

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new InvalidOperationException("Ошибка сервера (500). Обратитесь к главному эксперту.");
                }

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync(cancellationToken);
                return ParseFullName(content);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or InvalidOperationException)
            {
                errors.Add($"{endpoint}: {ex.Message}");
            }
        }

        throw new InvalidOperationException("Не удалось получить ФИО из эмулятора. " + string.Join("; ", errors));
    }

    private static string ParseFullName(string content)
    {
        string trimmedContent = content.Trim();

        if (string.IsNullOrWhiteSpace(trimmedContent))
        {
            throw new InvalidOperationException("API вернул пустой ответ.");
        }

        try
        {
            using JsonDocument document = JsonDocument.Parse(trimmedContent);
            JsonElement root = document.RootElement;

            if (root.ValueKind == JsonValueKind.String)
            {
                return NormalizeFullName(root.GetString());
            }

            if (root.ValueKind == JsonValueKind.Object)
            {
                foreach (string propertyName in new[] { "value", "fullName", "name" })
                {
                    if (root.TryGetProperty(propertyName, out JsonElement property)
                        && property.ValueKind == JsonValueKind.String)
                    {
                        return NormalizeFullName(property.GetString());
                    }
                }

                throw new InvalidOperationException("В ответе API не найдено поле value/fullName/name.");
            }
        }
        catch (JsonException)
        {
            return NormalizeFullName(trimmedContent);
        }

        throw new InvalidOperationException("API вернул ФИО в неподдерживаемом формате.");
    }

    private static string NormalizeFullName(string? fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new InvalidOperationException("API вернул пустое значение ФИО.");
        }

        return fullName.Trim();
    }

    private sealed class FullNameResponse
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}
