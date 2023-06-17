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
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;

        public RoleController(IRoleService roleService, IPermissionService permissionService)
        {
            _roleService = roleService;
            _permissionService = permissionService;
        }

        [AuthorizeCode("System:Role:Page")]
        public IActionResult Index()
        {
            return View();
        }

        [AuthorizeCode("System:Role:AddOrUpdate")]
        public async Task<IActionResult> Edit(long id)
        {
            return View((await _roleService.GetRoleAsync(id)).Result);
        }

        [AuthorizeCode("System:Role:AssignFunctions")]
        public IActionResult RoleAuthorization(long id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        [AuthorizeCode("System:Role:AddOrUpdate")]
        public async Task<MessageModel> AddRole(string name, string remark)
            => await _roleService.AddRoleAsync(name, remark);

        [HttpPost, AuthorizeCode("System:Role:Remove")]
        public async Task<MessageModel> RemoveRole(long id)
             => await _roleService.RemoveRoleAsync(id);

        [HttpPost, AuthorizeCode("System:Role:Query")]
        public async Task<MessageModel> GetRoleList([FromBody] RoleSearch search)
            => await _roleService.GetRoleListAsync(search);

        [HttpPost]
        [AuthorizeCode("System:Role:AddOrUpdate")]
        public async Task<MessageModel> UpdateRole(long id, string name, string remark)
            => await _roleService.UpdateRoleAsync(id, name, remark);

        [HttpPost, AuthorizeCode("System:Role:AssignFunctions")]
        public async Task<MessageModel> SetRoleFunctions([FromForm] long id, [FromForm] IList<long> funcIds)
        {
            return await _permissionService.SetRoleFunctionsAsync(id, funcIds);
        }

        [HttpPost, AuthorizeCode("System:Role:AssignFunctions")]
        public async Task<MessageModel<IList<long>>> GetRoleFunctionIds(long id)
            => await _permissionService.GetRoleFunctionIdsAsync(id);
    }
}