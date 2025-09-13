namespace SocialMedia.DAL.Entity
{
    public class Connection
    {
        public int Id { get; set; }

        public string SenderId { get; set; }
        public User Sender { get; set; }

        public string ReceiverId { get; set; }
        public User Receiver { get; set; }
        public bool IsBlocked { get; set; } = false;

        public ConnectionStatus Status { get; set; } = ConnectionStatus.Pending;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }

    public enum ConnectionStatus
    {
        Pending = 0,
        Accepted = 1,
        Rejected = 2,
        Blocked = 3
    }
}