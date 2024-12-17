using Microsoft.AspNetCore.Mvc;

namespace AgendaWeb.Presentation.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
