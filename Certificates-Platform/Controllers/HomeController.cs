using Microsoft.AspNetCore.Mvc;

namespace Certificates_Platform.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Maps to Views/Home/Index.cshtml
        }
    }
}
