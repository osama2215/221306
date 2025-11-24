using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
