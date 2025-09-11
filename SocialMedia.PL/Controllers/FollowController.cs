using Microsoft.AspNetCore.Mvc;
using SocialMedia.DAL.DataBase;
using SocialMedia.DAL.Entity;
using Microsoft.AspNetCore.Identity;

namespace SocialMedia.PL.Controllers
{
    public class FollowController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SocialMediaDbContext _context;

        public FollowController(UserManager<User> userManager, SocialMediaDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> FollowUser(string followingId)
        {
            var currentUserId = _userManager.GetUserId(User);

            if (currentUserId == followingId)
                return BadRequest("You cannot follow yourself.");

            var follow = new Follow
            {
                FollowerId = currentUserId,
                FollowingId = followingId
            };

            _context.Follows.Add(follow);
            await _context.SaveChangesAsync();

            return RedirectToAction("Friends","Connection"); 
        }

        [HttpPost]
        public async Task<IActionResult> UnfollowUser(string followingId)
        {
            var currentUserId = _userManager.GetUserId(User);

            var follow = _context.Follows
                .FirstOrDefault(f => f.FollowerId == currentUserId && f.FollowingId == followingId);

            if (follow != null)
            {
                _context.Follows.Remove(follow);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
