using SocialMedia.BLL.ModelVM.Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.BLL.Service.Abstraction
{
    public interface IConnectionSerives

    {
        Task<bool> SendRequest(string senderId, string receiverId);
        Task<bool> AcceptRequest(int requestId, string receiverId);
        Task<bool> RejectRequest(int requestId, string receiverId);
        Task<List<ConnectionRequestVM>> GetRequests(string userId);
        Task<List<FriendVM>> GetFriends(string userId);
        Task<bool> AreConnected(string userA, string userB);
    }
}