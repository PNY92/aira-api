using AiraAPI.Interfaces;
using AiraAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace AiraAPI.Repositories
{
    public class OpenRouterRepository : IOpenRouterRepository
    {

        private string _apiKey = "";

        public async IAsyncEnumerable<Message> GenerateMessageAsync(Message client_message)
        {

            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new Exception("API Key is not set");
            }

            using HttpClient client = new HttpClient();
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://openrouter.ai/api/v1/chat/completions");

            request.Headers.Add("Authorization", "Bearer " + _apiKey);
            request.Content = new StringContent($@"
            {{
                ""model"": ""deepseek/deepseek-chat:free"",
                ""messages"": [
                    {{
                        ""role"": ""user"",
                        ""content"": ""{client_message.Content}""
                    }}
                ],
                ""stream"": true
            }}", Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            using Stream stream = await response.Content.ReadAsStreamAsync();
            using StreamReader reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                string line = await reader.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(line) && line.StartsWith("data: "))
                {
                    string json = line.Substring(6).Trim();
                    if (json == "[DONE]") break; // OpenAI-style end signal

                    var responseObject = JsonConvert.DeserializeObject<Response>(json);
                    if (responseObject?.Choices != null && responseObject.Choices.Count > 0)
                    {
                        if (responseObject.Choices[0].Delta.Content != null)
                        {
                            yield return responseObject.Choices[0].Delta;
                        }
                        
                        
                    }
                }
            }

            yield return new Message();
        }

        public void SetAPIKey(string api_key)
        {
            _apiKey = api_key;
        }
    }
}
