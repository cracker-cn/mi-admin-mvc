using Mi.Core.Attributes;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.Workspace.Controllers
{
    [Area("Workspace")]
    [Authorize]
    public class DashboardController : Controller
    {
        [AuthorizeCode("Dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
