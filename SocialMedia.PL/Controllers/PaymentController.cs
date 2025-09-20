using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SocialMedia.DAL.Entity;
using System.IO;
using System.Threading.Tasks;

namespace SocialMedia.PL.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(UserManager<User> userManager, ILogger<PaymentController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        // GET: /Payment/Index
        [AllowAnonymous]
        public IActionResult Index()
        {
            // ViewBag.PublishableKey = _stripeSettings.PublishableKey;
            return View();
        }

        // POST: /Payment/CreateCheckoutSession
        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession(decimal amountUsd = 50.00m)
        {
            // Stripe integration commented out for now
            return RedirectToAction("Index");
        }

        // GET: /Payment/Success
        [AllowAnonymous]
        public IActionResult Success()
        {
            // Note: webhook is the reliable source-of-truth; this page is for UX.
            return View();
        }

        // GET: /Payment/Cancel
        [AllowAnonymous]
        public IActionResult Cancel()
        {
            return View();
        }

        // POST: /Payment/Webhook  <- set this URL in Stripe (e.g., https://yourdomain.com/Payment/Webhook)
        [HttpPost]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken] // Stripe posts without antiforgery token
        public async Task<IActionResult> Webhook()
        {
            // Stripe webhook commented out for now
            return Ok();
        }
    }
}