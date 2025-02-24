using Microsoft.AspNetCore.Mvc;

namespace AiraAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class IndexController : Controller
    {
        public IActionResult Index()
        {
            return Ok("200 Success");
        }
    }
}
