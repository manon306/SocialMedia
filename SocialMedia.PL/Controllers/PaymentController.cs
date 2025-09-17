using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SocialMedia.BLL.Config;
using SocialMedia.DAL.Entity;
using Stripe;
using Stripe.Checkout;
using Stripe.V2;
using System.IO;
using System.Threading.Tasks;

namespace SocialMedia.PL.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly StripeSettings _stripeSettings;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IOptions<StripeSettings> stripeSettings, UserManager<User> userManager, ILogger<PaymentController> logger)
        {
            _stripeSettings = stripeSettings.Value;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: /Payment/Index
        [AllowAnonymous]
        public IActionResult Index()
        {
            ViewBag.PublishableKey = _stripeSettings.PublishableKey;
            return View();
        }

        // POST: /Payment/CreateCheckoutSession
        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession(decimal amountUsd = 50.00m)
        {
            // Get the current logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var domain = $"{Request.Scheme}://{Request.Host}";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                ClientReferenceId = user.Id, // links session to our user
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(amountUsd * 100), // amount in cents
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Premium Membership"
                            }
                        },
                        Quantity = 1
                    }
                },
                SuccessUrl = domain + "/Payment/Success",
                CancelUrl = domain + "/Payment/Cancel"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            // Redirect the user to Stripe Checkout
            return Redirect(session.Url ?? "/");
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
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var sigHeader = Request.Headers["Stripe-Signature"];
            var webhookSecret = _stripeSettings.WebhookSecret;

            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, sigHeader, webhookSecret);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Webhook signature verification failed.");
                return BadRequest();
            }

            // Handle the event
            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                // The client_reference_id contains the User.Id we set when creating the session
                var clientReferenceId = session?.ClientReferenceId;

                if (!string.IsNullOrEmpty(clientReferenceId))
                {
                    var user = await _userManager.FindByIdAsync(clientReferenceId);
                    if (user != null)
                    {
                        user.IsPremium = true;
                        var result = await _userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            _logger.LogWarning("Failed to update IsPremium for user {UserId}", clientReferenceId);
                        }
                        else
                        {
                            _logger.LogInformation("User {UserId} marked as premium", clientReferenceId);
                        }
                    }
                }
            }

            // Respond 200 to Stripe
            return Ok();
        }
    }
}