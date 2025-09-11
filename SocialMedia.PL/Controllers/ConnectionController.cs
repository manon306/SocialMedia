using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.BLL.ModelVM.Connect;
using SocialMedia.BLL.Service.Implementation;
using SocialMedia.DAL.DataBase;
using SocialMedia.DAL.Entity;
using System.Security.Claims;

namespace SocialMedia.PL.Controllers
{
    [Authorize]
    public class ConnectionController : Controller
    {
        private readonly IConnectionSerives _service;
        private readonly UserManager<User> _userManager;
        private readonly SocialMediaDbContext _db;


        public ConnectionController(IConnectionSerives service, UserManager<User> userManager, SocialMediaDbContext _db)
        {
            _service = service;
            _userManager = userManager;
            this._db = _db;
        }



        [HttpGet]
        public async Task<IActionResult> Requests()
        {
            var me = _userManager.GetUserId(User);
            var model = await _service.GetRequests(me);
            return View(model); // Views/Connection/Requests.cshtml
        }

      

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SendRequest(string receiverId)
        //{
        //    var me = _userManager.GetUserId(User);
        //    await _service.SendRequest(me, receiverId);
        //    return Redirect(Request.Headers["Referer"].ToString());
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendRequest(string receiverId)
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (senderId == receiverId)
                return BadRequest("You cannot send a request to yourself.");

            var result = await _service.SendRequest(senderId, receiverId);

            if (result)
                return RedirectToAction("Friends", "Connection"); 
            else
                return BadRequest("Request already exists or failed.");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptRequest(int requestId)
        {
            var me = _userManager.GetUserId(User);
            await _service.AcceptRequest(requestId, me);
            return RedirectToAction(nameof(Requests));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            var me = _userManager.GetUserId(User);
            await _service.RejectRequest(requestId, me);
            return RedirectToAction(nameof(Requests));
        }

        public async Task<IActionResult> AllUsers()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var users = await _userManager.Users
                .Where(u => u.Id != currentUserId)
                .ToListAsync();

            var model = users.Select(u => new FriendVM
            {
                Id = u.Id,
                Name = u.Name,
                Headline = u.Headline,
                ProfileImagePath = u.ImagePath,
                Email = u.Email   //
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Friends()
        {
            //var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var friends = await _service.GetMyFriends(currentUserId);
            //return View(friends);
            var me = _userManager.GetUserId(User);
            var model = await _service.GetFriends(me);
            return View(model); // Views/Connection/Friends.cshtml
        }
        [HttpGet]
        public async Task<IActionResult> Search(string keyword)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var users = await _userManager.Users
                .Where(u => u.Id != currentUserId &&
                           (u.Name.Contains(keyword) || u.Email.Contains(keyword)))
                .ToListAsync();

            if (!users.Any())
            {
                ViewBag.Message = "No User Found😢";
            }

            var model = users.Select(u => new FriendVM
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Headline = u.Headline,
                ProfileImagePath = u.ImagePath
            }).ToList();

          

            return View("AllUsers", model);
        }

        [HttpPost]
        public async Task<IActionResult> Block(string friendId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var result = await _service.BlockFriend(userId, friendId);

            if (!result)
                return BadRequest("Could not block user.");

            return RedirectToAction("Friends"); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unblock(string friendId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var connection = await _db.Connections
                .FirstOrDefaultAsync(c =>
                    (c.SenderId == userId && c.ReceiverId == friendId) ||
                    (c.ReceiverId == userId && c.SenderId == friendId));

            if (connection == null)
            {
                return NotFound();
            }

            //connection.IsBlocked = false;       //to delete blolck
            connection.Status = ConnectionStatus.Pending;
            _db.Connections.Update(connection);
            await _db.SaveChangesAsync();

            return RedirectToAction("Requests");
        }


    }
}
