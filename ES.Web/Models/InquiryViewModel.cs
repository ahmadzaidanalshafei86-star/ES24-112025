namespace ES.Web.Models
{
    public class InquiryViewModel
    {
        public int TenderId { get; set; }

        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Inquiry { get; set; } = null!;
        public IFormFile? Attachment { get; set; }
        public string TenderSlug { get; set; } = "";
    }
}
