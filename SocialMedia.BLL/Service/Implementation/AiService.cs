using System.Text;
using System.Text.Json;

namespace SocialMedia.BLL.Service.Implementation
{
    public class AiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "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";

        // المُنشئ الصحيح مع حقن HttpClient
        public AiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api-inference.huggingface.co/");
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey.Trim());
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<string> AskAiAsync(string prompt)
        {
            try
            {
                var request = new
                {
                    inputs = prompt
                };

                var content = new StringContent(JsonSerializer.Serialize(request),
                                                Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("models/microsoft/DialoGPT-medium", content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return $"API Error: {response.StatusCode} - {result}";

                try
                {
                    using var doc = JsonDocument.Parse(result);
                    var generatedText = doc.RootElement[0].GetProperty("generated_text").GetString();
                    return generatedText ?? "No response from AI.";
                }
                catch
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}