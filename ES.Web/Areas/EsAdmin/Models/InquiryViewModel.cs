namespace ES.Web.Areas.EsAdmin.Models
{
    public class InquiryViewModel
    {

        public int Id { get; set; }
     


        public int TenderId { get; set; }
        public string TenderName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string InquiryText { get; set; } = null!;
        // للعرض فقط (download)
        public string? AttachmentUrl { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
