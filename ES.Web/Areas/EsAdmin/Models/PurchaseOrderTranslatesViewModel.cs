using AKM.Core.Entities;

namespace ES.Web.Areas.EsAdmin.Models
{
    public class PurchaseOrderTranslatesViewModel
    {
        public int? PurchaseOrderId { get; set; }
        public string? PurchaseOrderTitle { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? PurchaseOrderDefaultLang { get; set; }

        public IEnumerable<PurchaseOrderTranslate>? PreEnteredTranslations { get; set; }
    }
}
