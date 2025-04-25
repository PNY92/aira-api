using AiraAPI.Models.Source;
using AiraAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AiraAPI.Controllers
{

    [ApiController]
    [Route("api/paper")]
    public class SourceController : Controller
    {

        private SemanticScholarRepository _semanticScholarRepository = new SemanticScholarRepository();

        private HttpClient _httpClient;

        [HttpGet("autocomplete")]
        public async Task<IActionResult> AutoComplete([FromQuery] string query)
        {

            Paper paper = await _semanticScholarRepository.GetPaperAutoCompleteAsync(query);

            List<PaperInfo> paperInfo = await _semanticScholarRepository.GetPaperInfoAsync(paper);

            if (paperInfo.Count == 0)
            {
                return new StatusCodeResult(429);
            }
            return Ok(paperInfo);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            PaperSearch paper = await _semanticScholarRepository.SearchPaperAsync(query);

            List<PaperInfo> paperInfo = await _semanticScholarRepository.GetPaperInfoAsync(paper);

            if (paperInfo.Count == 0)
            {
                return new StatusCodeResult(429);
            }

            return Ok(paperInfo);
        }

    }
}
