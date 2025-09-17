using Microsoft.AspNetCore.Mvc;
using SocialMedia.BLL.Service.Implementation;
using System.Threading.Tasks;

namespace SocialMedia.PL.Controllers
{
    public class AiController : Controller
    {
        private readonly AiService _aiService;

        public AiController(AiService aiService)
        {
            _aiService = aiService;
        }

        [HttpGet]
        public IActionResult SuggestPost()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SuggestPost(string topic)
        {
            var suggestion = await _aiService.GeneratePostSuggestionAsync(topic);
            ViewBag.Suggestion = suggestion;
            return View();
        }
    }
}