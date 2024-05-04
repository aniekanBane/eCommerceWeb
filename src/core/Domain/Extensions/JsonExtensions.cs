using System.Text.Json;

namespace eCommerceWeb;

public static class JsonExtensions
{
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static T? FromJsonString<T>(this string text)
    {
        return JsonSerializer.Deserialize<T>(text, _serializerOptions);
    }

    public static string ToJsonString(this object obj)
    {
        return JsonSerializer.Serialize(obj, _serializerOptions);
    }
}
