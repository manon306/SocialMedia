namespace SocialMedia.DAL.REPO.Abstraction
{
    public interface IConnectionRepo
    {
        Task<bool> Exists(string userA, string userB);
        Task<Connection?> GetConnection(string userA, string userB);
        Task<Connection?> GetById(int id);
        Task<List<Connection>> GetUserConnections(string userId, ConnectionStatus? status = null);
        Task Add(Connection connection);
        Task Update(Connection connection);
        Task SaveChanges();
    }
}