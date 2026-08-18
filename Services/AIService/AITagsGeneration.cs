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
public async Task<string> GetUpdatedInterestTagForUser(string allTags)
{
    try
    {
        var prompt = $@"
Below are tags collected from a user's recent liked and bookmarked chats.

Your task is to identify the user's strongest and most relevant interests.

IMPORTANT:
You must ONLY select tags that already exist in the provided User's Recent Interest Tags.

Rules:
- Return only 8-10 tags
- Every output tag must come directly from the User's Recent Interest Tags
- Do NOT generate new tags
- Do NOT change spelling
- Do NOT correct spelling
- Do NOT create synonyms
- Do NOT modify, merge, or transform any tag
- Keep the exact same spelling as it appears in the input
- Prefer tags that appear frequently or represent the user's strongest interests
- Remove duplicates
- lowercase only
- space separated
- no commas
- no numbering
- no explanation
- Return only the final selected tags

User's Recent Interest Tags:
{allTags}
";

        var requestBody = new
        {
           model = "openai/gpt-oss-20b",
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

        var apiKey = _configuration["Groq:UserNewTags"];

        Console.WriteLine(
            $"API Key exists: {!string.IsNullOrWhiteSpace(apiKey)}"
        );

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.groq.com/openai/v1/chat/completions"
        );

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        request.Content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.SendAsync(request);

        // Read the response BEFORE throwing an exception
        var responseBody = await response.Content.ReadAsStringAsync();

        // Console.WriteLine($"Status Code: {response.StatusCode}");
        // Console.WriteLine($"Response Body: {responseBody}");

        if (!response.IsSuccessStatusCode)
        {
            return "";
        }

        using var doc = JsonDocument.Parse(responseBody);

        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return content?.Trim() ?? "";
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            $"Error generating interest tags: {ex.Message}"
        );

        return "";
    }
}    public async Task<string> GenerateTagsAsync(
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
    model = "openai/gpt-oss-20b",
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
