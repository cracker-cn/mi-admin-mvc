using Mi.Admin.WebComponent.Filter;
using Mi.Core.Models;
using Mi.Core.Models.UI;
using Mi.IService.System;
using Mi.IService.System.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NuGet.Packaging;

namespace Mi.Admin.Areas.System.Controllers
{
    [Area("System")]
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;

        public RoleController(IRoleService roleService, IPermissionService permissionService)
        {
            _roleService = roleService;
            _permissionService = permissionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(long id)
        {
            return View((await _roleService.GetRoleAsync(id)).Result);
        }

        public IActionResult RoleAuthorization(long id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public async Task<MessageModel> AddRole(string name, string remark)
            => await _roleService.AddRoleAsync(name, remark);

        [HttpPost]
        public async Task<MessageModel> RemoveRole(long id)
             => await _roleService.RemoveRoleAsync(id);

        [HttpPost]
        public async Task<MessageModel> GetRoleList([FromBody] RoleSearch search)
            => await _roleService.GetRoleListAsync(search);

        [HttpPost]
        public async Task<MessageModel> UpdateRole(long id, string name, string remark)
            => await _roleService.UpdateRoleAsync(id, name, remark);

        [HttpPost]
        public async Task<MessageModel> SetRoleFunctions([FromForm] long id, [FromForm] IList<long> funcIds)
        {
            return await _permissionService.SetRoleFunctionsAsync(id, funcIds);
        }

        [HttpPost]
        public async Task<MessageModel<IList<long>>> GetRoleFunctionIds(long id)
            => await _permissionService.GetRoleFunctionIdsAsync(id);
    }
}