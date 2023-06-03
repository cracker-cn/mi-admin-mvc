using Mi.Core.Service;

using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.Account.Controllers
{
    [Area("Account")]
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}