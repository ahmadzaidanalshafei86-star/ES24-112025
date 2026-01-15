namespace ES.Web.Models
{
    public class PurchaseOrdersArchiveViewViewModel
    {
        public List<PurchaseOrder> PurchaseOrdersArchive { get; set; } = new List<PurchaseOrder>();
        public string? Slug { get; set; }
        public string? Title { get; set; }
    }
}
