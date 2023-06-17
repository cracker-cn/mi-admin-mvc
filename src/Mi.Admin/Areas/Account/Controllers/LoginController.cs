using Mi.Core.Models;
using Mi.Core.Service;
using Mi.IService.System;

using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.Account.Controllers
{
    [Area("Account")]
    public class LoginController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly HttpContext _httpContext;

        public LoginController(IPermissionService permissionService, IHttpContextAccessor httpContextAccessor)
        {
            _permissionService = permissionService;
            _httpContext = httpContextAccessor.HttpContext!;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<MessageModel> Do(string userName, string password, string code)
        {
            var result = await _permissionService.LoginAsync(userName, password, code);
            var logService = DotNetService.Get<ILogService>();
            await logService.WriteLogAsync(userName, result.EnsureSuccess(), result.Message ?? "");
            return result;
        }

        [HttpPost]
        public async Task<MessageModel> New(string userName, string password)
            => await _permissionService.RegisterAsync(userName, password);

        public async Task<IActionResult> Exit()
        {
            await _permissionService.LogoutAsync();
            return Redirect("/Account/Login");
        }
    }
}