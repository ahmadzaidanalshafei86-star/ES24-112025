namespace ES.Web.Areas.EsAdmin.Models
{
    public class BookServiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? EmailAddress { get; set; }
        public string? Notes { get; set; }
        public string BranchName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public List<string> Hangars { get; set; } = new();
        public List<string> Refrigators { get; set; } = new();
    }
}
