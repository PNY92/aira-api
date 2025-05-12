using AiraAPI.Models;
using AiraAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AiraAPI.Controllers
{
    [Route("/api/llm")]
    public class LLMController : Controller
    {
        [HttpPost("tune")]
        public IActionResult Tune([FromBody] TuningPrompt tuningPrompt)
        {
            Console.WriteLine(tuningPrompt.Prompt);
            ConfigManager.SetSystemPrompt(tuningPrompt.Prompt);
            return Ok();
        }
    }
}
