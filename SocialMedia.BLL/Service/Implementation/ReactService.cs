namespace SocialMedia.BLL.Service.Implementation
{
    public class ReactService : IReactService
    {
        private readonly IReactRepo reactRepo;
        private readonly IMapper mapper;

        public ReactService(IReactRepo reactRepo, IMapper mapper)
        {
            this.reactRepo = reactRepo;
            this.mapper = mapper;
        }

        public (bool, string) AddReact(AddReactVm addReactVm)
        {
            if (addReactVm == null)
                return (false, "React cannot be null");

            var react = new React(addReactVm.Type, addReactVm.CreatedBy, addReactVm.PostID);

            var result = reactRepo.AddReact(react);
            return result;
        }

        public (bool, string) ToggleReact(AddReactVm reactVm)
        {
            // الحصول على التفاعل الحالي للمستخدم إذا موجود
            var existingReactResult = reactRepo.GetUserReactForPost(reactVm.PostID, reactVm.CreatedBy);

            if (existingReactResult.Item1) // إذا كان هناك تفاعل موجود
            {
                // إذا كان نفس النوع، يتم حذف التفاعل (إلغاء التفاعل)
                if (existingReactResult.Item3.Type == reactVm.Type)
                {
                    return reactRepo.DeleteReact(existingReactResult.Item3.ID, reactVm.CreatedBy);
                }
                else // إذا كان نوع مختلف، يتم تحديث التفاعل
                {
                    existingReactResult.Item3.Update(reactVm.CreatedBy, reactVm.Type);
                    return reactRepo.UpdateReact(existingReactResult.Item3);
                }
            }
            else // إذا لم يكن هناك تفاعل، يتم إضافة تفاعل جديد
            {
                return AddReact(reactVm);
            }
        }

        public (bool, string, int) GetReactsCount(int postId)
        {
            var result = reactRepo.GetReactsByPostId(postId);
            if (!result.Item1)
                return (false, result.Item2, 0);

            return (true, null, result.Item3.Count);
        }

        public (bool, string, Dictionary<reactType, int>) GetReactsSummary(int postId)
        {
            var result = reactRepo.GetReactsByPostId(postId);
            if (!result.Item1)
                return (false, result.Item2, new Dictionary<reactType, int>());

            var summary = new Dictionary<reactType, int>();
            foreach (reactType type in Enum.GetValues(typeof(reactType)))
            {
                summary[type] = result.Item3.Count(r => r.Type == type);
            }

            return (true, null, summary);
        }

        public (bool, string, reactType?) GetUserReactType(int postId, string userId)
        {
            var result = reactRepo.GetUserReactForPost(postId, userId);
            if (!result.Item1)
                return (false, result.Item2, null);

            return (true, null, result.Item3.Type);
        }
    }
}