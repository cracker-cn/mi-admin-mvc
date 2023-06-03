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

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(long id)
        {
            return View((await _roleService.GetRoleAsync(id)).Result);
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
    }
}