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
                    Content = "A small healthcare clinic wants to implement a patient appointment booking system using Low-Code platform. The system must allow patients to:\r\n\r\n\r\nBook appointments online.\r\nReceive reminders via SMS.\r\nView doctor availability in real-time.\r\n\r\n\r\n1.     Justify which Low-Code platform would you recommend for this scenario? \r\n\r\n\r\n2.     What challenges might the company face while adopting Low-Code for this application? Propose solutions to address them.\r\n\r\n\r\n3.     Identify potential benefits for the clinic after implementing this system.\r\n\r\n\r\n4.     Evaluate any risks associated with this project and suggest mitigation strategies.",
                    Model = "deepseek/deepseek-chat:free",
                    Role = "User"
                };

                JObject configManager = ConfigManager.GetConfiguration();
                string key = configManager["openrouter"]["deepseek_v3_api"].ToString();

                OpenRouterRepository openRouterRepository = new OpenRouterRepository();
                openRouterRepository.SetAPIKey(key);

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

                /*
                Message chat = new Message()
                {
                    Content = "What are the advantages of using low-code?",
                    Model = "deepseek/deepseek-chat:free",
                    Role = "User"
                };
                JObject configManager = ConfigManager.GetConfiguration();
                string key = configManager["openrouter"]["deepseek_v3_api"].ToString();

                OpenRouterRepository openRouterRepository = new OpenRouterRepository();
                openRouterRepository.SetAPIKey(key);

                Task.Run(async () =>
                {
                    await foreach (var chunk in openRouterRepository.GenerateMessageAsync(chat))
                    {
                        Console.Write(chunk.Content);
                    }
                });


                return Ok("yes");
                */
            }
    }
}
