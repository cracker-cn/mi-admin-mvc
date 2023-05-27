using Microsoft.AspNetCore.Mvc;

namespace Mi.Admin.Areas.System.Controllers
{
	[Area("System")]
	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
