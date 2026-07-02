using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace AdvancedChat.Web.Services;

public class ApiService
{
    private readonly HttpClient _client;

    public ApiService(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri("https://localhost:7127/");
    }

    public async Task<T?> GetAsync<T>(string url, string token)
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return default;

        var json = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(json);
    }

    public async Task<T?> PostAsync<T>(
        string url,
        object model)
    {
        try
        {
            var json = JsonConvert.SerializeObject(model);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await _client.PostAsync(url, content);

            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"API Response Status: {response.StatusCode}");
            Console.WriteLine($"API Response Content: {result}");

            // Try to deserialize regardless of status code to get the error message
            try
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Error: {ex.Message}");
                Console.WriteLine(result);
                return default;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API Error: {ex.Message}");
            return default;
        }
    }
}
