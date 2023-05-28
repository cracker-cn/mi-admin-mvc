using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.Workspace.Controllers
{
    [Area("Workspace")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
