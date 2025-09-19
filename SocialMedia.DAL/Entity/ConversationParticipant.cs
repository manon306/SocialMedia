public sealed class ConversationParticipant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ConversationId { get; set; }
    public string UserId { get; set; } = default!;
    public DateTimeOffset JoinedAt { get; set; } = DateTimeOffset.UtcNow;

    public Conversation Conversation { get; set; } = default!;
}