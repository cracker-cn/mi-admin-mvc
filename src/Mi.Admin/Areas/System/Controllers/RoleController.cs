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
            throw new Exception("test");
            return View();
        }

        public async Task<IActionResult> Edit(long id)
        {
            return View((await _roleService.GetRoleAsync(id)).Result);
        }

        public IActionResult RoleFunction(long id)
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
        public async Task<MessageModel<IList<LayuiTreeModel>>> GetRoleFunctions(long id)
            => await _permissionService.GetRoleFunctionsAsync(id);

        [HttpPost]
        public async Task<MessageModel> SetRoleFunctions([FromForm] long id, [FromForm] IList<LayuiTreeModel> funcs)
        {
            var funcIds = GetIds(funcs);
            return await _permissionService.SetRoleFunctionsAsync(id, funcIds);
        }

        [NonAction]
        private IList<long> GetIds(IList<LayuiTreeModel> funcs)
        {
            var ids = new List<long>();
            foreach (var item in funcs)
            {
                if (!string.IsNullOrEmpty(item.Id))
                {
                    ids.Add(long.Parse(item.Id));
                    ids.AddRange(GetChildIds(item.Children));
                }
            }
            return ids;
        }

        [NonAction]
        private IList<long> GetChildIds(IList<LayuiTreeModel>? funcs)
        {
            var ids = new List<long>();
            if(funcs != null)
            {
                foreach (var item in funcs)
                {
                    if (!string.IsNullOrEmpty(item.Id))
                    {
                        ids.Add(long.Parse(item.Id));
                        ids.AddRange(GetChildIds(item.Children));
                    }
                }
            }
            return ids;
        }
    }
}