using Mi.Core.Models;
using Mi.Service.System;
using Mi.Service.System.Models;

using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.System.Controllers
{
	[Area("System")]
	public class UserController : Controller
	{
		private readonly UserService _userService;

		public UserController(UserService userService)
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