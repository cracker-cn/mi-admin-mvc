using Mi.Core.Attributes;
using Mi.Core.Models;
using Mi.IService.System;
using Mi.IService.System.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.System.Controllers
{
    [Area("System")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;

        public UserController(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        [AuthorizeCode("System:User")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(long id)
        {
            return View((await _userService.GetUserAsync(id)).Result);
        }

        public async Task<IActionResult> UserRole(long id)
        {
            ViewBag.Id = id;
            return View((await _permissionService.GetUserRolesAsync(id)).Result);
        }

        [HttpPost]
        [AuthorizeCode("System:User:List")]
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

        [HttpPost]
        public async Task<MessageModel> SetUserRole(long userId, List<long> roleIds)
            => await _permissionService.SetUserRoleAsync(userId, roleIds);

        [HttpPost]
        public async Task<MessageModel> PassedUser(long id)
            => await _userService.PassedUserAsync(id);
    }
}