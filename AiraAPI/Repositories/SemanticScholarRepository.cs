using AiraAPI.Interfaces;
using AiraAPI.Models.Source;
using FireSharp.Extensions;
using Newtonsoft.Json;

namespace AiraAPI.Repositories
{
    public class SemanticScholarRepository : ISemanticScholarRepository
    {
        private HttpClient _httpClient;
        public SemanticScholarRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task<Paper> GetPaperAutoCompleteAsync(string query)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://api.semanticscholar.org/graph/v1/paper/autocomplete?query={query}");

            using HttpResponseMessage httpResponse = await _httpClient.SendAsync(request);

            if (httpResponse.StatusCode == (System.Net.HttpStatusCode)429)
            {
                return new Paper();
            }
            HttpContent response = httpResponse.EnsureSuccessStatusCode().Content;

            if (response == null)
            {
                throw new NullReferenceException("Response Content is null here");
            }


            Paper paper = JsonConvert.DeserializeObject<Paper>(response.ReadAsStringAsync().Result);

            return paper;

        }

        public async Task<List<PaperInfo>> GetPaperInfoAsync(Paper paper)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://api.semanticscholar.org/graph/v1/paper/batch?fields=title,url,abstract,isOpenAccess,openAccessPdf,authors");

            List<string> ids = new List<string>();

            if (paper.Matches == null)
            {
                return new List<PaperInfo>();
            }

            for (int i = 0; i < paper.Matches.Count; i++)
            {
                ids.Add(paper.Matches[i].Id);
            }

            request.Content = new StringContent($@"
            {{""ids"": {ids.ToJson()}}}
            ");


            using HttpResponseMessage httpResponse = await _httpClient.SendAsync(request);

            if (httpResponse.StatusCode == (System.Net.HttpStatusCode)429)
            {
                return new List<PaperInfo>();
            }

            HttpContent response = httpResponse.EnsureSuccessStatusCode().Content;


            List<PaperInfo> paperInfo = JsonConvert.DeserializeObject<List<PaperInfo>>(response.ReadAsStringAsync().Result);

            return paperInfo;
        }

        public async Task<List<PaperInfo>> GetPaperInfoAsync(PaperSearch paper)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://api.semanticscholar.org/graph/v1/paper/batch?fields=title,url,abstract,isOpenAccess,openAccessPdf,authors");

            List<string> ids = new List<string>();

            if (paper.Data == null)
            {
                return new List<PaperInfo>();
            }

            for (int i = 0; i < paper.Data.Count; i++)
            {
                ids.Add(paper.Data[i].paperId);
            }

            request.Content = new StringContent($@"
            {{""ids"": {ids.ToJson()}}}
            ");


            using HttpResponseMessage httpResponse = await _httpClient.SendAsync(request);

            HttpContent response = httpResponse.EnsureSuccessStatusCode().Content;


            List<PaperInfo> paperInfo = JsonConvert.DeserializeObject<List<PaperInfo>>(response.ReadAsStringAsync().Result);

            return paperInfo;
        }

        public async Task<PaperSearch> SearchPaperAsync(string query)
        {

            HttpResponseMessage httpResponse = new HttpResponseMessage();
            while (true)
            {
                using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://api.semanticscholar.org/graph/v1/paper/search?query={query}&offset=0&limit=10");

                httpResponse = await _httpClient.SendAsync(request);

                if (httpResponse.IsSuccessStatusCode)
                {
                    break;
                }
                await Task.Delay(3000);

            }
            

            HttpContent response = httpResponse.EnsureSuccessStatusCode().Content;

            if (response == null)
            {
                throw new NullReferenceException("Response Content is null here");
            }


            PaperSearch paper = JsonConvert.DeserializeObject<PaperSearch>(response.ReadAsStringAsync().Result);

            return paper;
        }
    }
}
