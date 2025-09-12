

namespace SocialMedia.DAL.REPO.IMPLEMENTATION
{
    public class ConnectionRepo: IConnectionRepo
    {

        private readonly SocialMediaDbContext _db;

        public ConnectionRepo(SocialMediaDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Exists(string userA, string userB)
        {
            return await _db.Connections.AnyAsync(c =>
                (c.SenderId == userA && c.ReceiverId == userB) ||
                (c.SenderId == userB && c.ReceiverId == userA));
        }

        public async Task<Connection?> GetConnection(string userA, string userB)
        {
            return await _db.Connections.FirstOrDefaultAsync(c =>
                (c.SenderId == userA && c.ReceiverId == userB) ||
                (c.SenderId == userB && c.ReceiverId == userA));
        }

        public async Task<Connection?> GetById(int id)
        {
            return await _db.Connections.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Connection>> GetUserConnections(string userId, ConnectionStatus? status = null)
        {
            var query = _db.Connections.AsQueryable();

            if (status.HasValue)
                query = query.Where(c => c.Status == status);

            return await query
                .Where(c => c.SenderId == userId || c.ReceiverId == userId)
                .Include(c => c.Sender)
                .Include(c => c.Receiver)
                .ToListAsync();
        }

        public async Task Add(Connection connection)
        {
            await _db.Connections.AddAsync(connection);
        }

        //public async Task Update(Connection connection)
        //{
        //    _db.Connections.Update(connection);
        //    await Task.CompletedTask;
        //}
        public Task Update(Connection connection)
        {
            _db.Connections.Update(connection);
            return Task.CompletedTask;
        }


        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}
