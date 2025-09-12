using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SocialMedia.BLL.ModelVM.Connect;
using SocialMedia.DAL.DataBase;


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

            //return connections
            //    .Where(c => c.ReceiverId == userId)
            //    .AsQueryable()
            //    .ProjectTo<ConnectionRequestVM>(_mapper.ConfigurationProvider)
            //    .ToList();
            return connections
    .Where(c => c.ReceiverId == userId)
    .Select(c => _mapper.Map<ConnectionRequestVM>(c))
    .ToList();

        }

        // Get Friends
        //public async Task<List<FriendVM>> GetFriends(string userId)
        //{
        //    var connections = await _repo.GetUserConnections(userId, ConnectionStatus.Accepted);

        //    var friends = connections
        //        .Select(c => c.SenderId == userId ? c.Receiver : c.Sender)
        //        .AsQueryable();

        //    return await friends.ProjectTo<FriendVM>(_mapper.ConfigurationProvider).ToListAsync();
        //}
        public async Task<List<FriendVM>> GetFriends(string userId)
        {
            var connections = await _repo.GetUserConnections(userId, ConnectionStatus.Accepted);

            var friends = connections
                .Select(c => c.SenderId == userId ? c.Receiver : c.Sender)
                .Select(f => _mapper.Map<FriendVM>(f))
                .ToList();

            return friends;
        }


        //    //get friend see your friend ,show only pepole are connected
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






        #region   //    private readonly SocialMediaDbContext _db;
        //    private readonly IMapper _mapper;

        //    public ConnectionSerives(SocialMediaDbContext db, IMapper mapper)
        //    {
        //        _db = db;
        //        _mapper = mapper;
        //    }


        //    //send request
        //    public async Task<bool> SendRequest(string senderId, string receiverId)
        //    {
        //        if (senderId == receiverId) return false;

        //        var exists = await _db.Connections.AnyAsync(c =>
        //            (c.SenderId == senderId && c.ReceiverId == receiverId) ||
        //            (c.SenderId == receiverId && c.ReceiverId == senderId));

        //        if (exists) return false;

        //        _db.Connections.Add(new Connection
        //        {
        //            SenderId = senderId,
        //            ReceiverId = receiverId,
        //            Status = ConnectionStatus.Pending
        //        });

        //        await _db.SaveChangesAsync();
        //        return true;
        //    }


        //    //Accept request
        //    public async Task<bool> AcceptRequest(int requestId, string receiverId)
        //    {
        //        var req = await _db.Connections.FirstOrDefaultAsync(c => c.Id == requestId && c.ReceiverId == receiverId);
        //        if (req == null) return false;

        //        req.Status = ConnectionStatus.Accepted;
        //        await _db.SaveChangesAsync();
        //        return true;
        //    }


        //    //rject
        //    public async Task<bool> RejectRequest(int requestId, string receiverId)
        //    {
        //        var req = await _db.Connections.FirstOrDefaultAsync(c => c.Id == requestId && c.ReceiverId == receiverId);
        //        if (req == null) return false;

        //        req.Status = ConnectionStatus.Rejected;
        //        await _db.SaveChangesAsync();
        //        return true;
        //    }


        //    //get requests
        //    public async Task<List<ConnectionRequestVM>> GetRequests(string userId)
        //    {
        //        return await _db.Connections
        //            .AsNoTracking()
        //            .Where(c => c.ReceiverId == userId && c.Status == ConnectionStatus.Pending)
        //            .Include(c => c.Sender)
        //            .ProjectTo<ConnectionRequestVM>(_mapper.ConfigurationProvider)
        //            .ToListAsync();
        //    }


        //    //get friend see all friend ,show all pepole
        //    public async Task<List<FriendVM>> GetFriends(string userId)
        //    {
        //        var q = _db.Connections.AsNoTracking()
        //            .Where(c => c.Status == ConnectionStatus.Accepted &&
        //                        (c.SenderId == userId || c.ReceiverId == userId))
        //            .Select(c => c.SenderId == userId ? c.Receiver : c.Sender);

        //        return await q.ProjectTo<FriendVM>(_mapper.ConfigurationProvider).ToListAsync();
        //    }



        //    public async Task<bool> AreConnected(string userA, string userB)
        //    {
        //        return await _db.Connections.AnyAsync(c =>
        //            c.Status == ConnectionStatus.Accepted &&
        //            ((c.SenderId == userA && c.ReceiverId == userB) ||
        //             (c.SenderId == userB && c.ReceiverId == userA)));
        //    }


        //    //get friend see your friend ,show only pepole are connected

        //    public async Task<List<FriendVM>> GetMyFriends(string userId)
        //    {
        //        var connections = await _db.Connections
        //            .Where(c => (c.SenderId == userId || c.ReceiverId == userId) && c.Status == ConnectionStatus.Accepted)
        //            .Include(c => c.Sender)
        //            .Include(c => c.Receiver)
        //            .ToListAsync();

        //        var friends = connections.Select(c =>
        //        {
        //            var friend = c.SenderId == userId ? c.Receiver : c.Sender;
        //            return new FriendVM
        //            {
        //                Id = friend.Id,
        //                Name = friend.Name,
        //               // Email = friend.Email,
        //                Headline = friend.Headline,
        //                 ProfileImagePath = friend.ImagePath

        //            };
        //        }).ToList();

        //        return friends;
        //    }




        //    //block friend
        //    public async Task<bool> BlockFriend(string userId, string friendId)
        //    {
        //        var connection = await _db.Connections
        //            .FirstOrDefaultAsync(c =>
        //                (c.SenderId == userId && c.ReceiverId == friendId) ||
        //                (c.SenderId == friendId && c.ReceiverId == userId));

        //        if (connection == null)
        //            return false;

        //        connection.Status = ConnectionStatus.Blocked;
        //        await _db.SaveChangesAsync();
        //        return true;
        //    }

        //    public async Task<bool> UnblockFriend(string userId, string friendId)
        //    {
        //        var connection = await _db.Connections
        //            .FirstOrDefaultAsync(c =>
        //                (c.SenderId == userId && c.ReceiverId == friendId) ||
        //                (c.ReceiverId == userId && c.SenderId == friendId));

        //        if (connection == null)
        //            return false;

        //        connection.Status = ConnectionStatus.Pending;
        //        connection.IsBlocked = false;

        //        _db.Connections.Update(connection);
        //        await _db.SaveChangesAsync();

        //        return true;
        //    }


        //}
        #endregion
    }
}

