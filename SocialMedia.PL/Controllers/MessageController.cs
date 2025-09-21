public class MessageController : Controller
{
    private readonly IHubContext<ChatHub> _hubContext;

    public MessageController(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public async Task<IActionResult> Send(string user, string message)
    {
        // لو عايزة تستدعي BLL هنا وتخزني الرسالة في DB
        // messageService.SaveMessage(user, message);

        await _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);
        return Ok();
    }
}
