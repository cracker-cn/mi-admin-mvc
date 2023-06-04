using Mi.Core.Models;
using Mi.IService.System;

using Microsoft.AspNetCore.Authentication;
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
            => await _permissionService.LoginAsync(userName, password, code);

        [HttpPost]
        public async Task<MessageModel> New(string userName, string password)
            => await _permissionService.RegisterAsync(userName, password);

        public async Task<IActionResult> Exit()
        {
            await _httpContext.SignOutAsync();
            _httpContext.Features.Set(new UserModel());
            return Redirect("/Account/Login");
        }
    }
}