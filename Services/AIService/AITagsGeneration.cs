using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FindYOU;

public class AITagsGeneration
{
      private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AITagsGeneration(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> GenerateTagsAsync(
        string title,
        string category,
        string? summary,
        string? notes)
    {
       var prompt = $@"
Generate 5-8 relevant tags.

Rules:
- lowercase only
- space separated
- no commas
- no numbering
- no explanation

Title:
{title}

Category:
{category}

Summary:
{summary}

Notes:
{notes}

Example Output:
dotnet aspnet mvc efcore postgresql
";

var requestBody = new
{
    model = "llama-3.1-8b-instant",
    messages = new[]
    {
        new
        {
            role = "user",
            content = prompt
        }
    },
    temperature = 0.2
};

var json = JsonSerializer.Serialize(requestBody);
 var apiKey = _configuration["Groq:ApiKey"];

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.groq.com/openai/v1/chat/completions");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(responseBody);

        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return content?.Trim() ?? "";

    }
}
