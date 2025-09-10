using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SocialMedia.BLL.ModelVM.Connect;
using SocialMedia.DAL.DataBase;
using System.Data;


namespace SocialMedia.BLL.Service.Implementation
{
    public class ConnectionSerives : IConnectionSerives
    {
        private readonly SocialMediaDbContext _db;
        private readonly IMapper _mapper;

        public ConnectionSerives(SocialMediaDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> SendRequest(string senderId, string receiverId)
        {
            if (senderId == receiverId) return false;

            var exists = await _db.Connections.AnyAsync(c =>
                (c.SenderId == senderId && c.ReceiverId == receiverId) ||
                (c.SenderId == receiverId && c.ReceiverId == senderId));

            if (exists) return false;

            _db.Connections.Add(new Connection
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Status = ConnectionStatus.Pending
            });

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AcceptRequest(int requestId, string receiverId)
        {
            var req = await _db.Connections.FirstOrDefaultAsync(c => c.Id == requestId && c.ReceiverId == receiverId);
            if (req == null) return false;

            req.Status = ConnectionStatus.Accepted;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectRequest(int requestId, string receiverId)
        {
            var req = await _db.Connections.FirstOrDefaultAsync(c => c.Id == requestId && c.ReceiverId == receiverId);
            if (req == null) return false;

            req.Status = ConnectionStatus.Rejected;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<ConnectionRequestVM>> GetRequests(string userId)
        {
            return await _db.Connections
                .AsNoTracking()
                .Where(c => c.ReceiverId == userId && c.Status == ConnectionStatus.Pending)
                .Include(c => c.Sender)
                .ProjectTo<ConnectionRequestVM>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<FriendVM>> GetFriends(string userId)
        {
            var q = _db.Connections.AsNoTracking()
                .Where(c => c.Status == ConnectionStatus.Accepted &&
                            (c.SenderId == userId || c.ReceiverId == userId))
                .Select(c => c.SenderId == userId ? c.Receiver : c.Sender);

            return await q.ProjectTo<FriendVM>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<bool> AreConnected(string userA, string userB)
        {
            return await _db.Connections.AnyAsync(c =>
                c.Status == ConnectionStatus.Accepted &&
                ((c.SenderId == userA && c.ReceiverId == userB) ||
                 (c.SenderId == userB && c.ReceiverId == userA)));
        }


    }

}