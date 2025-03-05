using AiraAPI.Interfaces;
using AiraAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace AiraAPI.Repositories
{
    public class OpenRouterClient : IOpenRouterClient
    {

        private string _apiKey = "";
        private readonly HttpClient _httpClient;

        public delegate void CompleteResponse();

        public event CompleteResponse OnResponseCompleted;

        public OpenRouterClient(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
        }

        public async IAsyncEnumerable<Message> GenerateStreamingMessageAsync(Message client_message)
        {

            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new Exception("API Key is not set");
            }

            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://openrouter.ai/api/v1/chat/completions");

            request.Headers.Add("Authorization", "Bearer " + _apiKey);

            request.Content = new StringContent($@"
            {{
                ""model"": ""{client_message.Model}"",
                ""messages"": [
                    {{
                        ""role"": ""user"",
                        ""content"": ""{client_message.Content}""
                    }}
                ],
                ""stream"": true
            }}", Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            using Stream stream = await response.Content.ReadAsStreamAsync();
            using StreamReader reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                string line = await reader.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(line) && line.StartsWith("data: "))
                {
                    string json = line.Substring(6).Trim();

                    if (json == "[DONE]")
                    { // OpenAI-style end signal
                        OnResponseCompleted?.Invoke();
                        break;
                    } 

                    

                    Response responseObject = JsonConvert.DeserializeObject<Response>(json);
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

        public async Task<string> GenerateMessageAsync(Message client_message)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new Exception("API Key is not set");
            }

            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://openrouter.ai/api/v1/chat/completions");

            request.Headers.Add("Authorization", "Bearer " + _apiKey);

            request.Content = new StringContent($@"
            {{
                ""model"": ""{client_message.Model}"",
                ""messages"": [
                    {{
                        ""role"": ""user"",
                        ""content"": ""{client_message.Content}""
                    }}
                ],
            }}", Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _httpClient.SendAsync(request);
            var responsesCode = response.EnsureSuccessStatusCode();

            string content = await responsesCode.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            Response AI_Response = JsonConvert.DeserializeObject<Response>(content);

            return AI_Response.Choices[0].Message.Content;
        }
    }
}
