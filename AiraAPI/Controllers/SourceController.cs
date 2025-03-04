using Microsoft.AspNetCore.Mvc;

namespace AiraAPI.Controllers
{
    public class SourceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
