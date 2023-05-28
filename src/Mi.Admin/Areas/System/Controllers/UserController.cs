using Mi.Core.Models;
using Mi.IService.System;
using Mi.IService.System.Models;

using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.System.Controllers
{
	[Area("System")]
	public class UserController : Controller
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		public IActionResult Index()
		{
			return View();
		}

        public async Task<IActionResult> Edit(long id)
        {
            return View((await _userService.GetUserAsync(id)).Result);
        }

        [HttpPost]
		public async Task<MessageModel> GetUserList([FromBody] UserSearch search)
		{
			return await _userService.GetUserListAsync(search);
		}

		[HttpPost]
		public async Task<MessageModel> AddUser(string userName)
		{
			return await _userService.AddUserAsync(userName);
		}

		[HttpPost]
		public async Task<MessageModel> RemoveUser(long userId)
		{
			return await _userService.RemoveUserAsync(userId);
		}

		[HttpPost]
		public async Task<MessageModel> UpdatePassword(long userId)
		{
			return await _userService.UpdatePasswordAsync(userId);
		}
    }
}