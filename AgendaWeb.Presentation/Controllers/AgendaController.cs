using Microsoft.AspNetCore.Mvc;

namespace AgendaWeb.Presentation.Controllers
{
    public class AgendaController : Controller
    {
        public IActionResult Cadastro()
        {
            return View();
        }

        public IActionResult Consulta()
        {
            return View();
        }

        public IActionResult Edicao()
        {
            return View();
        }

        public IActionResult Relatorio()
        {
            return View();
        }
    }
}
