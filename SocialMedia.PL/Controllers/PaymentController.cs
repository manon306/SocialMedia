using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class PaymentController : Controller
{
    private readonly PaymobService _paymob;

    public PaymentController(PaymobService paymob)
    {
        _paymob = paymob;
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
}
