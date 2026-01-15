namespace ES.Web.Models
{
    public class TendersViewViewModel
    {
        public List<Tender> Tenders { get; set; } = new List<Tender>();
        public string? Slug { get; set; }
        public string? Title { get; set; }
    }

}




