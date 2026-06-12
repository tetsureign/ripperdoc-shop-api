using System.Text;
using System.Text.Json;

namespace RipperdocShop.Web.Services;

public abstract class BaseApiService(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _http = httpClientFactory.CreateClient("CustomerApi");

    protected async Task<T?> GetAsync<T>(string endpoint, Dictionary<string, string>? queryParams = null)
    {
        if (queryParams is not null && queryParams.Count != 0)
        {
            var query = string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
            endpoint = $"{endpoint}?{query}";
        }
        
        var res = await _http.GetAsync(endpoint);
        
        if (!res.IsSuccessStatusCode) return default;
        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
    
    // POST with body
    protected async Task<T?> PostAsync<T>(string endpoint, object data)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(data),
            Encoding.UTF8,
            "application/json"
        );

        var res = await _http.PostAsync(endpoint, content);
        if (!res.IsSuccessStatusCode) return default;

        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    // POST with no return
    protected async Task<bool> PostAsync(string endpoint, object data)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(data),
            Encoding.UTF8,
            "application/json"
        );

        var res = await _http.PostAsync(endpoint, content);
        return res.IsSuccessStatusCode;
    }
}
