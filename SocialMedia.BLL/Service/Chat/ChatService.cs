using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SocialMedia.DAL.DataBase;
using SocialMedia.DAL.Entity;

namespace SocialMedia.BLL.Service.Chat
{
    public interface IChatService
    {
        Task<Conversation> GetOrCreateDirectConversationAsync(string userAId, string userBId, CancellationToken ct = default);
        Task<Conversation> GetConversationAsync(Guid conversationId, CancellationToken ct = default);
        Task<bool> IsParticipantAsync(Guid conversationId, string userId, CancellationToken ct = default);
        Task<Message> SaveMessageAsync(Guid conversationId, string senderUserId, string body, CancellationToken ct = default);
        Task<IReadOnlyList<Message>> GetRecentMessagesAsync(Guid conversationId, int take, CancellationToken ct = default);
    }

    public sealed class ChatService : IChatService
    {
        private readonly SocialMediaDbContext _db;
        public ChatService(SocialMediaDbContext db) => _db = db;

        public async Task<Conversation> GetOrCreateDirectConversationAsync(string userAId, string userBId, CancellationToken ct = default)
        {
            var pair = new HashSet<string>(StringComparer.Ordinal) { userAId, userBId };

            var convo = await _db.Conversations
                .AsNoTracking()
                .Where(c => !c.IsGroup)
                .Where(c => _db.ConversationParticipants
                    .Where(p => p.ConversationId == c.Id)
                    .Select(p => p.UserId)
                    .Distinct()
                    .Count() == 2)
                .FirstOrDefaultAsync(c =>
                    _db.ConversationParticipants
                        .Where(p => p.ConversationId == c.Id)
                        .Select(p => p.UserId)
                        .All(uid => pair.Contains(uid)), ct);

            if (convo is not null) return convo;

            convo = new Conversation { IsGroup = false };
            _db.Conversations.Add(convo);
            _db.ConversationParticipants.Add(new ConversationParticipant { ConversationId = convo.Id, UserId = userAId });
            _db.ConversationParticipants.Add(new ConversationParticipant { ConversationId = convo.Id, UserId = userBId });
            await _db.SaveChangesAsync(ct);
            return convo;
        }

        public Task<Conversation> GetConversationAsync(Guid conversationId, CancellationToken ct = default) =>
            _db.Conversations.AsNoTracking().FirstAsync(c => c.Id == conversationId, ct);

        public Task<bool> IsParticipantAsync(Guid conversationId, string userId, CancellationToken ct = default) =>
            _db.ConversationParticipants.AnyAsync(p => p.ConversationId == conversationId && p.UserId == userId, ct);

        public async Task<Message> SaveMessageAsync(Guid conversationId, string senderUserId, string body, CancellationToken ct = default)
        {
            var msg = new Message { ConversationId = conversationId, SenderUserId = senderUserId, Body = body };
            _db.Messages.Add(msg);
            await _db.SaveChangesAsync(ct);
            return msg;
        }

        public async Task<IReadOnlyList<Message>> GetRecentMessagesAsync(Guid conversationId, int take, CancellationToken ct = default)
        {
            var items = await _db.Messages
                .AsNoTracking()
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.CreatedAt)
                .Take(take)
                .ToListAsync(ct);

            items.Reverse(); // chronological ascending
            return items;
        }
    }
}