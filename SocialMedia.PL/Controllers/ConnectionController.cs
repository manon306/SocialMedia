using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.DAL.Entity;
using System.Security.Claims;

namespace SocialMedia.PL.Controllers
{
    public class ConnectionController : Controller
    {
        private readonly IConnectionSerives _service;
        private readonly UserManager<User> _userManager;

        public ConnectionController(IConnectionSerives service, UserManager<User> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Requests()
        {
            var me = _userManager.GetUserId(User);
            var model = await _service.GetRequests(me);
            return View(model); // Views/Connection/Requests.cshtml
        }

        [HttpGet]
        public async Task<IActionResult> Friends()
        {
            var me = _userManager.GetUserId(User);
            var model = await _service.GetFriends(me);
            return View(model); // Views/Connection/Friends.cshtml
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
    }
}
