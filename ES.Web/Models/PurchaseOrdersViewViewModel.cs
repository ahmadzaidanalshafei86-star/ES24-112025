namespace ES.Web.Models
{
    public class PurchaseOrdersViewViewModel
    {
        public List<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
        public string? Slug { get; set; }
        public string? Title { get; set; }
    }
}
