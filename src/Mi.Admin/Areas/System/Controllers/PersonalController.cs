using Mi.Core.Attributes;
using Mi.Core.Models;
using Mi.Core.Models.UI;
using Mi.IService.System;
using Mi.IService.System.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.System.Controllers
{
    [Area("System")]
    [Authorize]
    public class PersonalController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly IUserService _userService;

        public PersonalController(IPermissionService permissionService, IUserService userService)
        {
            _permissionService = permissionService;
            _userService = userService;
        }

        [AuthorizeCode("System:Personal:Page")]
        public IActionResult Index()
        {
            return View();
        }

        [AuthorizeCode("System:Personal:CutImage")]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<List<PaMenuModel>> GetSiderMenu() => await _permissionService.GetSiderMenuAsync();

        [HttpPost]
        [AllowAnonymous]
        public async Task<MessageModel<UserBaseInfo>> GetUserBaseInfo() => await _userService.GetUserBaseInfoAsync();

        [HttpPost, AuthorizeCode("System:Personal:SetUserBaseInfo")]
        public async Task<MessageModel> SetUserBaseInfo([FromBody] UserBaseInfo model) => await _userService.SetUserBaseInfoAsync(model);

        [HttpPost, AuthorizeCode("System:Personal:SetPassword")]
        public async Task<MessageModel> SetPassword(string password) => await _userService.SetPasswordAsync(password);
    }
}