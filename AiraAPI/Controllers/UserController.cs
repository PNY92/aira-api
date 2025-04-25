using AiraAPI.Models;
using AiraAPI.Repositories;
using FireSharp.Config;
using Microsoft.AspNetCore.Mvc;

namespace AiraAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {

        private FirebaseConfig _firebaseConfig = new FirebaseConfig()
        {
            AuthSecret = ConfigManager.GetConfiguration()["firebase_config"]["apiKey"].ToString(),
            BasePath = ConfigManager.GetConfiguration()["firebase_config"]["basePath"].ToString()
        };

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            Response.Headers.ContentType = "application/json";

            FireBaseRepository fireBaseRepository = new FireBaseRepository(_firebaseConfig);
            User user = await fireBaseRepository.GetUserAsync();
            return new JsonResult(user);
        }

        [HttpGet("update")]
        public async Task<IActionResult> Update()
        {
            Response.Headers.ContentType = "application/json";

            FireBaseRepository fireBaseRepository = new FireBaseRepository(_firebaseConfig);
            User user = await fireBaseRepository.GetUserAsync();
            return new JsonResult(user);
        }

        [HttpGet("delete")]
        public async Task<IActionResult> Delete()
        {
            Response.Headers.ContentType = "application/json";

            FireBaseRepository fireBaseRepository = new FireBaseRepository(_firebaseConfig);
            User user = await fireBaseRepository.GetUserAsync();
            return new JsonResult(user);
        }
    }
}
