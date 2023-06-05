using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.Account.Controllers
{
    [Area("Account")]
    public class FailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
