using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class PaymentController : Controller
{
    private readonly PaymobService _paymob;
    private readonly UserManager<SocialMedia.DAL.Entity.User> _userManager;

    public PaymentController(PaymobService paymob , UserManager<SocialMedia.DAL.Entity.User> _userManager )
    {
        _paymob = paymob;
        this._userManager = _userManager;
    }

    [HttpGet("pay")]
    public async Task<IActionResult> Pay()
    {
        var authToken = await _paymob.GetAuthTokenAsync();
        var orderId = await _paymob.CreateOrderAsync(authToken, 10000, "EGP"); // 100 جنيه = 10000 قرش
        var paymentToken = await _paymob.GetPaymentKeyAsync(authToken, orderId, 10000, "EGP");

        // ضيف الـ Iframe ID بتاعك من Paymob Dashboard
        var iframeUrl = $"https://accept.paymob.com/api/acceptance/iframes/960894?payment_token={paymentToken}";

        return Redirect(iframeUrl);
    }
    [HttpGet("payment/success")]
    public async Task<IActionResult> PaymentSuccess(string id)
    {
        // هات الـ User الحالي
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.IsPremium = true;
                await _userManager.UpdateAsync(user);
            }
        }

        TempData["Message"] = "تمت ترقية حسابك إلى Premium ✅";
        return RedirectToAction("Index", "Home");
    }

    // 3️⃣ لو فشل الدفع
    [HttpGet("payment/fail")]
    public IActionResult PaymentFail()
    {
        TempData["Message"] = "فشل الدفع ❌";
        return RedirectToAction("Index", "Home");
    }
}
