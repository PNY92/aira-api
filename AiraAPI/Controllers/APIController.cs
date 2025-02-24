using Microsoft.AspNetCore.Mvc;
using AiraAPI.Models;
using AiraAPI.Repositories;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace AiraAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class APIController : Controller
    {
        
        public IActionResult Index()
        {
            Message chat = new Message()
            {
                Content = "Hello",
                Model = "deepseek/deepseek-chat:free",
                Role = "User"
            };
            JObject configManager = ConfigManager.GetConfiguration();
            string key = configManager["openrouter"]["deepseek_v3_api"].ToString();

            OpenRouterRepository openRouterRepository = new OpenRouterRepository();
            openRouterRepository.SetAPIKey(key);
            Task<Message> responseStatus = openRouterRepository.GenerateMessageAsync(chat);

            Message responseMessage = responseStatus.Result;

            string resultJson = JsonConvert.SerializeObject(responseMessage);

            return Ok(resultJson);
        }
    }
}
