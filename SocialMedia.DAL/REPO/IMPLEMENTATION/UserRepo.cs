namespace SocialMedia.DAL.REPO.IMPLEMENTATION
{
    public class UserRepo : IuserRepo
    {
        private readonly SocialMediaDbContext db;

        public UserRepo(SocialMediaDbContext db)
        {
            db = db;
        }

        public bool Create(User user)
        {
            try
            {
                var result = db.Users.Add(user);
                db.SaveChanges();
                return result.Entity.Id != null;
            }
            catch
            {
                return false;
            }
        }

        public List<User> GetUsers()
        {
            return db.Users.ToList();
        }
        public List<User> SearchUser(string keyword)
        {
            return db.Users.Where(u => u.Name.Contains(keyword) || u.Email.Contains(keyword)).ToList();

        }

    }
}