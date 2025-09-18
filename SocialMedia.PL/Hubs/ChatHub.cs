using Microsoft.AspNetCore.SignalR;

public sealed class ChatHub : Hub
{
    private readonly IChatService _chat;
    public ChatHub(IChatService chat) => _chat = chat;

    public override async Task OnConnectedAsync()
    {
        // Optional: add connection to a dedicated user group for broadcast-by-user
        if (Context.UserIdentifier is string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
        }
        await base.OnConnectedAsync();
    }

    public async Task SendToUser(string targetUserId, string body)
    {
        if (string.IsNullOrWhiteSpace(body)) return;
        var senderId = Context.UserIdentifier ?? Guid.NewGuid().ToString(); // fallback for testing
        var convo = await _chat.GetOrCreateDirectConversationAsync(senderId, targetUserId);
        var saved = await _chat.SaveMessageAsync(convo.Id, senderId, body);

        await Clients.User(targetUserId).SendAsync("ReceiveDirectMessage", new
        {
            conversationId = convo.Id,
            senderUserId = senderId,
            body = saved.Body,
            sentAt = saved.CreatedAt
        });
        await Clients.User(senderId).SendAsync("ReceiveDirectMessage", new
        {
            conversationId = convo.Id,
            senderUserId = senderId,
            body = saved.Body,
            sentAt = saved.CreatedAt
        });
    }

    public async Task SendToGroup(Guid conversationId, string body)
    {
        if (string.IsNullOrWhiteSpace(body)) return;
        var userId = Context.UserIdentifier ?? Guid.NewGuid().ToString(); // fallback for testing
        if (!await _chat.IsParticipantAsync(conversationId, userId)) return;

        var saved = await _chat.SaveMessageAsync(conversationId, userId, body);
        await Clients.Group(conversationId.ToString()).SendAsync("ReceiveGroupMessage", new
        {
            conversationId,
            senderUserId = userId,
            body = saved.Body,
            sentAt = saved.CreatedAt
        });
    }

    public async Task JoinGroup(Guid conversationId)
    {
        var userId = Context.UserIdentifier ?? Guid.NewGuid().ToString(); // fallback for testing
        if (!await _chat.IsParticipantAsync(conversationId, userId)) return;
        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
    }

    public async Task LeaveGroup(Guid conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId.ToString());
    }

    public async Task LoadRecent(Guid conversationId, int take = 50)
    {
        var userId = Context.UserIdentifier ?? Guid.NewGuid().ToString(); // fallback for testing
        if (!await _chat.IsParticipantAsync(conversationId, userId)) return;

        var recent = await _chat.GetRecentMessagesAsync(conversationId, Math.Clamp(take, 1, 200));
        await Clients.Caller.SendAsync("RecentMessages", conversationId, recent);
    }
}