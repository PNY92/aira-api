using AiraAPI.Models.Source;

namespace AiraAPI.Interfaces
{
    public interface ISemanticScholarRepository
    {
        public Task<Paper> GetPaperAutoCompleteAsync(string query);

        public Task<PaperSearch> SearchPaperAsync(string query);

        public Task<List<PaperInfo>> GetPaperInfoAsync(Paper paper);

        public Task<List<PaperInfo>> GetPaperInfoAsync(PaperSearch paper);
    }
}
