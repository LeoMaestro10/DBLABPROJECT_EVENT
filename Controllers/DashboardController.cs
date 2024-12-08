using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;

namespace WebApplication1.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult index()
        {
            return View();
        }
    }
}
