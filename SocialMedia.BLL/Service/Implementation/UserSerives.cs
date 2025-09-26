namespace SocialMedia.BLL.Service.Implementation
{
    public class UserSerives : IUserSerives
    {
        private readonly IuserRepo userRepo;
        private readonly IMapper mapper;

        public UserSerives(IMapper mapper, IuserRepo userRepo)
        {
            this.userRepo = userRepo;
            this.mapper = mapper;
        }

        public (bool, string, List<ViewProfileVM>) Search(string keyword)
        {
            try
            {
                var users = userRepo.SearchUser(keyword);
                var result = mapper.Map<List<ViewProfileVM>>(users);
                return (false, "Sucess", result);
            }
            catch (Exception ex)
            {
                return (true, ex.Message, null);

            }

        }
        public (bool, string, List<ViewProfileVM>) GetAll()
        {
            try
            {
                var users = userRepo.GetUsers();
                // var result = mapper.Map<List<GetAllUserVM>>(users);
                List<ViewProfileVM> result = new();
                foreach (var useritem in users)
                {
                    result.Add(new ViewProfileVM()
                    {
                        // headline , bio ,location ,skills
                        Name = useritem.Name,
                        ProfileImagePath = useritem.ImagePath,
                        Headline = useritem.Headline,
                        Bio = useritem.Bio,
                        Location = useritem.Location

                    });
                }
                return (false, null, result);
            }
            catch (Exception ex)
            {
                return (true, ex.Message, null);

            }
        }


        public (bool success, string message, List<ViewProfileVM> suggestedUsers) GetSuggestUsers(string id)
        {
            try
            {
                var currentUser = userRepo.GetByID(id);
                if (currentUser == null)
                    return (false, "User not found", new List<ViewProfileVM>());

                // جلب كل المستخدمين ما عدا اليوزر الحالي
                var allUsers = userRepo.GetUsers().Where(u => u.Id != id).ToList();

                // فلترة المستخدمين اللي عندهم نفس Headline
                var usersWithSameHeadline = allUsers
                    .Where(u => u.Bio?.ToLower() == currentUser.Bio?.ToLower())
                    .ToList();

                // تحويل إلى ViewModel
                var result = usersWithSameHeadline.Select(user => new ViewProfileVM
                {
                    Name = user.UserName,
                    Education = user.Education,
                    Headline = user.Headline,
                    Bio = user.Bio
                }).ToList();

                return (true, $"Found {result.Count} users with similar headline", result);
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}", new List<ViewProfileVM>());
            }
        }
    }
}