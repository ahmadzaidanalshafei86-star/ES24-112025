


using AKM.Core.Entities;

namespace ES.Web.Areas.EsAdmin.Models
{
    public class TenderTranslatesViewModel
    {
        public int? TenderId { get; set; }
        public string? TenderTitle { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? TenderDefaultLang { get; set; }

        public IEnumerable<TenderTranslate>? PreEnteredTranslations { get; set; }
    }
}
