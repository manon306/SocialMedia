using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class PaymobService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "ZXlKaGJHY2lPaUpJVXpVeE1pSXNJblI1Y0NJNklrcFhWQ0o5LmV5SmpiR0Z6Y3lJNklrMWxjbU5vWVc1MElpd2ljSEp2Wm1sc1pWOXdheUk2TVRBM056TXdNU3dpYm1GdFpTSTZJbWx1YVhScFlXd2lmUS53a1lMOEFsVjVjODBuWFZQMmh1QnhwSjU0SXZpRDhtZ1cwMTNpdm5QYmhhekc1T1ZNNDdlNkV3OE5LMC1pMDBmT2lhVzhCUUJVaWd2WGdfS2NyZFhzUQ==";  // ضيفي الـ API Key بتاعك هنا
    private readonly int _integrationId = 5301175;           // Integration ID 

    public PaymobService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // 1. Get Auth Token
    public async Task<string> GetAuthTokenAsync()
    {
        var body = new { api_key = _apiKey };
        var json = JsonConvert.SerializeObject(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://accept.paymob.com/api/auth/tokens", content);
        var result = await response.Content.ReadAsStringAsync();

        dynamic obj = JsonConvert.DeserializeObject(result);
        return obj.token;
    }

    // 2. Create Order
    public async Task<int> CreateOrderAsync(string token, int amountCents, string currency)
    {
        var body = new
        {
            auth_token = token,
            delivery_needed = "false",
            amount_cents = amountCents.ToString(),
            currency = currency,
            items = new object[] { },
            redirect_url ="~/Post"
        };

        var json = JsonConvert.SerializeObject(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://accept.paymob.com/api/ecommerce/orders", content);
        var result = await response.Content.ReadAsStringAsync();

        dynamic obj = JsonConvert.DeserializeObject(result);
        return obj.id;
    }

    // 3. Get Payment Key
    public async Task<string> GetPaymentKeyAsync(string token, int orderId, int amountCents, string currency)
    {
        var billingData = new
        {
            apartment = "NA",
            email = "customer@test.com",
            floor = "NA",
            first_name = "Test",
            last_name = "User",
            street = "NA",
            building = "NA",
            phone_number = "+201234567890",
            shipping_method = "NA",
            postal_code = "NA",
            city = "Cairo",
            country = "EG",
            state = "NA"
        };

        var body = new
        {
            auth_token = token,
            amount_cents = amountCents,
            expiration = 3600,
            order_id = orderId,
            billing_data = billingData,
            currency = currency,
            integration_id = _integrationId
        };

        var json = JsonConvert.SerializeObject(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://accept.paymob.com/api/acceptance/payment_keys", content);
        var result = await response.Content.ReadAsStringAsync();

        dynamic obj = JsonConvert.DeserializeObject(result);
        return obj.token;
    }
}
