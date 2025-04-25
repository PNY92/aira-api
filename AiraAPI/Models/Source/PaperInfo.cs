

namespace AiraAPI.Models.Source
{
    public class PaperInfo
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }

        public OpenAccessPDF OpenAccessPDF { get; set; }
    }
}
