using Mi.Core.Models.UI;
using Mi.IService.System;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.System.Controllers
{
    [Area("System")]
    [Authorize]
    public class PersonalController : Controller
    {
        private readonly IPermissionService _permissionService;

        public PersonalController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpPost]
        public async Task<List<PaMenuModel>> GetSiderMenu()
            => await _permissionService.GetSiderMenuAsync();
    }
}