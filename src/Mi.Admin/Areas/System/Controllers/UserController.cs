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

		[HttpPost]
		public async Task<MessageModel> GetUserList([FromBody] UserSearch search)
		{
			return await _userService.GetUserListAsync(search);
		}
	}
}