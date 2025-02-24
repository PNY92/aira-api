using Microsoft.AspNetCore.Mvc;
using AiraAPI.Models;
using AiraAPI.Repositories;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;

namespace AiraAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class APIController : Controller
    {

        [HttpGet("get")]
        public async Task Index()
        {
            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");

            var responseStream = Response.Body;

            await using (StreamWriter writer = new StreamWriter(responseStream, Encoding.UTF8, leaveOpen: true))
            {
                Message chat = new Message()
                {
                    Content = "Can you explain what is delegate in C#?",
                    Model = "deepseek/deepseek-chat:free",
                    Role = "User"
                };

                JObject configManager = ConfigManager.GetConfiguration();
                string key = configManager["openrouter"]["deepseek_v3_api"].ToString();

                OpenRouterClient openRouterRepository = new OpenRouterClient(key);

                openRouterRepository.OnResponseCompleted += () =>
                {
                    Console.WriteLine("\nResponse Completed.");
                };

                await foreach (var chunk in openRouterRepository.GenerateMessageAsync(chat))
                {
                    Console.Write(chunk.Content);
                    if (!string.IsNullOrEmpty(chunk.Content))
                    {
                        
                        await writer.WriteAsync(chunk.Content);
                        await writer.FlushAsync();
                    }
                }

                
            }

        }
    }
}
