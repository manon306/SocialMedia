using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SocialMedia.Configuration;
using SocialMedia.PL.Configuration;

namespace SocialMedia.Controllers
{
    public class MapController : Controller
    {
        private readonly GoogleMapsConfig _mapsConfig;

        public MapController(IOptions<GoogleMapsConfig> mapsOptions)
        {
            _mapsConfig = mapsOptions.Value;
        }

        public IActionResult Index()
        {
            ViewBag.GoogleMapsApiKey = _mapsConfig.ApiKey;
            return View();
        }
    }
}