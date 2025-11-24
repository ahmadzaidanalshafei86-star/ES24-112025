


namespace ES.Web.Areas.EsAdmin.Models
{
    public class BranchTranslatesViewModel
    {
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? BranchDefaultLang { get; set; }

        public IEnumerable<BranchTranslate>? PreEnteredTranslations { get; set; }
    }
}

