namespace ES.Web.Models
{
    public class TendersArchiveViewViewModel
    {
        public List<Tender> TendersArchive { get; set; } = new List<Tender>();
        public string? Slug { get; set; }
        public string? Title { get; set; }
    }
}
