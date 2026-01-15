


using ES.Core.Entities;

namespace AKM.Core.Entities
{
    public class PurchaseOrderTranslate
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string? Details { get; set; }
        public string? PricesOffered { get; set; }


        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Foreign Keys
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public int LanguageId { get; set; }
        public Language Language { get; set; } = null!;
    }
}
