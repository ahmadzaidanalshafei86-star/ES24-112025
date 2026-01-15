using ES.Web.Services;

namespace ES.Web.Controllers.client
{
    public class PurchaseOrdersArchiveViewController : Controller
    {
        private readonly PurchaseOrdersArchiveViewService _purchaseOrdersArchiveViewService;

        public PurchaseOrdersArchiveViewController(PurchaseOrdersArchiveViewService purchaseOrdersArchiveViewService)
        {
            _purchaseOrdersArchiveViewService = purchaseOrdersArchiveViewService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _purchaseOrdersArchiveViewService.GetAllAsync();

            return View(model);
        }
    }
}
