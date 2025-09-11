namespace SocialMedia.DAL.REPO.IMPLEMENTATION
{
    public class ReactRepo : IReactRepo
    {
        private readonly SocialMediaDbContext DB;
        public ReactRepo(SocialMediaDbContext DB)
        {
            this.DB = DB;
        }

        public (bool, string) AddReact(React react)
        {
            if (react == null)
                return (false, "React cannot be null");

            // التحقق من عدم وجود تفاعل مسبق من نفس المستخدم على نفس المنشور
            var existingReact = DB.Reacts.FirstOrDefault(r =>
                r.PostID == react.PostID &&
                r.CreatedBy == react.CreatedBy &&
                !r.IsDeleted);

            if (existingReact != null)
                return (false, "User already reacted to this post");

            DB.Reacts.Add(react);
            DB.SaveChanges();
            return (true, null);
        }

        public (bool, string) UpdateReact(React react)
        {
            if (react == null)
                return (false, "React cannot be null");

            var existingReact = DB.Reacts.Find(react.ID);
            if (existingReact == null)
                return (false, "React not found");

            existingReact.Update(react.UpdatedBy, react.Type);
            DB.Reacts.Update(existingReact);
            DB.SaveChanges();
            return (true, null);
        }

        public (bool, string) DeleteReact(int reactId, string deletedBy)
        {
            var react = DB.Reacts.Find(reactId);
            if (react == null)
                return (false, "React not found");

            react.Delete(deletedBy);
            DB.Reacts.Update(react);
            DB.SaveChanges();
            return (true, null);
        }

        public (bool, string, React) GetReactById(int reactId)
        {
            var react = DB.Reacts.Find(reactId);
            if (react == null)
                return (false, "React not found", null);

            return (true, null, react);
        }

        public (bool, string, List<React>) GetReactsByPostId(int postId)
        {
            var reacts = DB.Reacts
                .Where(r => r.PostID == postId && !r.IsDeleted)
                .ToList();

            return (true, null, reacts);
        }

        public (bool, string, React) GetUserReactForPost(int postId, string userId)
        {
            var react = DB.Reacts
                .FirstOrDefault(r => r.PostID == postId && r.CreatedBy == userId && !r.IsDeleted);

            if (react == null)
                return (false, "No react found", null);

            return (true, null, react);
        }
    }
}