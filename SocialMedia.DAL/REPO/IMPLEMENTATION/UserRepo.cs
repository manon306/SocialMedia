


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
                var result = db.user.Add(user);
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
            return db.user.ToList();
        }

       
    }
}
