namespace SocialMedia.BLL.Service.Implementation
{
    public class ConnectionSerives : IConnectionSerives
    {
        private readonly IConnectionRepo _repo;
        private readonly IMapper _mapper;

        public ConnectionSerives(IConnectionRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        //    //send request
        public async Task<bool> SendRequest(string senderId, string receiverId)
        {
            if (senderId == receiverId) return false;
            if (await _repo.Exists(senderId, receiverId)) return false;

            var connection = new Connection
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Status = ConnectionStatus.Pending
            };

            await _repo.Add(connection);
            await _repo.SaveChanges();
            return true;
        }
        // Accept
        public async Task<bool> AcceptRequest(int requestId, string receiverId)
        {
            var req = await _repo.GetById(requestId);
            if (req == null || req.ReceiverId != receiverId) return false;

            req.Status = ConnectionStatus.Accepted;
            await _repo.Update(req);
            await _repo.SaveChanges();
            return true;
        }
        // Reject
        public async Task<bool> RejectRequest(int requestId, string receiverId)
        {
            var req = await _repo.GetById(requestId);
            if (req == null || req.ReceiverId != receiverId) return false;

            req.Status = ConnectionStatus.Rejected;
            await _repo.Update(req);
            await _repo.SaveChanges();
            return true;
        }

        //  Get Requests
        public async Task<List<ConnectionRequestVM>> GetRequests(string userId)
        {
            var connections = await _repo.GetUserConnections(userId, ConnectionStatus.Pending);
            return connections
         .Where(c => c.ReceiverId == userId)
         .Select(c => new ConnectionRequestVM
         {
             Id = c.Id,
             SenderId = c.Sender.Id,
             SenderName = c.Sender.Name,        // ✅ الاسم هيظهر
             SenderHeadline = c.Sender.Headline,
             SenderImage = c.Sender.ImagePath
         })
         .ToList();

        }
        public async Task<List<FriendVM>> GetFriends(string userId)
        {
            var connections = await _repo.GetUserConnections(userId, ConnectionStatus.Accepted);

            var friends = connections
                .Select(c => c.SenderId == userId ? c.Receiver : c.Sender)
                .Select(f => _mapper.Map<FriendVM>(f))
                .ToList();

            return friends;
        }
        //get friend see your friend ,show only pepole are connected
        public async Task<List<FriendVM>> GetMyFriends(string userId)
        {
            var connections = await _repo.GetUserConnections(userId, ConnectionStatus.Accepted);

            var friends = connections.Select(c =>
            {
                var friend = c.SenderId == userId ? c.Receiver : c.Sender;
                return new FriendVM
                {
                    Id = friend.Id,
                    Name = friend.Name,
                    Headline = friend.Headline,
                    ProfileImagePath = friend.ImagePath
                };
            }).ToList();

            return friends;


        }
        //  Block
        public async Task<bool> BlockFriend(string userId, string friendId)
        {
            var connection = await _repo.GetConnection(userId, friendId);
            if (connection == null) return false;

            connection.Status = ConnectionStatus.Blocked;
            await _repo.Update(connection);
            await _repo.SaveChanges();
            return true;
        }
        //  Unblock
        public async Task<bool> UnblockFriend(string userId, string friendId)
        {
            var connection = await _repo.GetConnection(userId, friendId);
            if (connection == null) return false;

            connection.Status = ConnectionStatus.Pending;
            connection.IsBlocked = false;

            await _repo.Update(connection);
            await _repo.SaveChanges();
            return true;
        }
        //  AreConnected
        public async Task<bool> AreConnected(string userA, string userB)
        {
            var conn = await _repo.GetConnection(userA, userB);
            return conn != null && conn.Status == ConnectionStatus.Accepted;
        }
    }
}