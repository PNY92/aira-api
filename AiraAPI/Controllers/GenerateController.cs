﻿using AiraAPI.Models;
using AiraAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;

namespace AiraAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class GenerateController : Controller
    {

        [HttpPost("generate")]
        public async Task Generate([FromBody] Message chatMessage)
        {
            chatMessage.Content += "\n\nPlease respond as a research assistant and provide accurate response";

            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");

            var responseStream = Response.Body;

            JObject configManager = ConfigManager.GetConfiguration();
            string key = configManager["openrouter"]["deepseek_v3_api"].ToString();

            OpenRouterClient openRouterRepository = new OpenRouterClient(key);

            openRouterRepository.OnResponseCompleted += () =>
            {
                Console.WriteLine("\nResponse Completed.");
            };

            await using (StreamWriter writer = new StreamWriter(responseStream, Encoding.UTF8, leaveOpen: true))
            {

                await foreach (var chunk in openRouterRepository.GenerateStreamingMessageAsync(chatMessage))
                {
                    if (!string.IsNullOrEmpty(chunk.Content))
                    {

                        await writer.WriteAsync(chunk.Content);
                        await writer.FlushAsync();
                    }
                }


            }

        }
        [HttpPost("source")]
        public async Task<IActionResult> Source([FromBody] Message chatMessage)
        {
            chatMessage.Content += "\n\nPlease summarize the user prompt into 3 to 5 keywords so that we can use as search query for Semantic Scholar without comma, it must reflect the whole context of the user prompt";

            Response.Headers.Add("Content-Type", "application/json");
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");

            var responseStream = Response.Body;

            JObject configManager = ConfigManager.GetConfiguration();
            string key = configManager["openrouter"]["deepseek_v3_api"].ToString();

            OpenRouterClient openRouterRepository = new OpenRouterClient(key);

            string content = await openRouterRepository.GenerateMessageAsync(chatMessage);
            string formattedContent = content.Replace(" ", "%20");


            return Ok(formattedContent);

        }
    }
}
