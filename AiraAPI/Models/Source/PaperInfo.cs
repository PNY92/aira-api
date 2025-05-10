

namespace AiraAPI.Models.Source
{

    public class Author
    {
        public string Name { get; set; }
    }

    public class ExternalIds
    {
        public string DOI { get; set; }
    }

    public class Venue
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class PaperInfo
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }

        public OpenAccessPDF OpenAccessPDF { get; set; }

        public string? PublicationDate { get; set; }

        public int Year { get; set; }

        public ExternalIds? ExternalIds { get; set; }

        public Venue? PublicationVenue { get; set; }

        public Author[]? Authors { get; set; }
    }
}
