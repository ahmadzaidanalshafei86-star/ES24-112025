namespace ES.Web.Models
{
    public class PurchaseOrderFilesViewModel
    {
        public string FileName { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public int DisplayOrder { get; set; }
    }
}
