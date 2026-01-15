
namespace ES.Web.Models
{
    public class PurchaseOrderInquiryViewModel
    {
        public int PurchaseOrderId { get; set; }

        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Inquiry { get; set; } = null!;
        public IFormFile? Attachment { get; set; }
        public string PurchaseOrderSlug { get; set; } = "";
    }
}
