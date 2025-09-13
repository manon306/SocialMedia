using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;

namespace YourApp.Controllers
{
    public class AiController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "IjJcbTE6R7d79jQ5LYjpvRxQvNkDSfmtJluCXLdO"; // ضيفي الـ API Key بتاع Cohere

        public AiController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://api.cohere.ai/v1/");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Ask(string userInput)
        {
            var requestBody = new
            {
                model = "command-r-plus",
                message = userInput   // ✅ Cohere بيستخدم "message" مش "messages"
            };

            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("chat", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Response = $"API Error: {response.StatusCode} - {responseString}";
                return View("AITOOL");
            }

            var json = JObject.Parse(responseString);
            var message = json["text"]?.ToString();   // ✅ استخراج الـ Response

            ViewBag.Response = message ?? "No response from AI.";
            return View("AITOOL");
        }


        [HttpGet]
        public IActionResult AITOOL()
        {
            return View();
        }
    }
}
