using System.Web.Mvc;
using TodoApp.Models;

namespace TodoApp.Controllers
{
	public class HomeController : Controller
	{
		[Authorize]
		public ActionResult Index()
		{
			var user = (User)User.Identity;
			return View(user);
		}
	}
}
