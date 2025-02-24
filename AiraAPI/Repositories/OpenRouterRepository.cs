using AiraAPI.Interfaces;
using AiraAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace AiraAPI.Repositories
{
    public class OpenRouterRepository : IOpenRouterRepository
    {

        private string _apiKey = "";

        public async Task<Message> GenerateMessageAsync(Message client_message)
        {

            if (_apiKey == null) {
                throw new Exception("API Key is not set");
            }
            
            HttpClient client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://openrouter.ai/api/v1/chat/completions");

            request.Headers.Add("Authorization", "Bearer " + _apiKey);

           
            request.Content = new StringContent($"{{\n  \"model\": \"deepseek/deepseek-chat:free\",\n  \"messages\": [\n    {{\n      \"role\": \"user\",\n      \"content\": \"{client_message.Content}\"\n    }}\n  ],\n \"stream\": true  \n}}");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/event-stream");

            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            
            Response respondedMessage = JsonConvert.DeserializeObject<Response>(responseBody);

            return respondedMessage.Choices[0].Message;
        }

        public void SetAPIKey(string api_key)
        {
            _apiKey = api_key;
        }
    }
}
