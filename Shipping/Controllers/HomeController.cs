using Microsoft.AspNetCore.Mvc;

namespace Shipping.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
