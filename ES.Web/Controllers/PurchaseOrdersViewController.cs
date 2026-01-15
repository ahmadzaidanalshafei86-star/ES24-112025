using ES.Web.Services;

namespace ES.Web.Controllers.client
{
    public class PurchaseOrdersViewController : Controller
    {
        private readonly PurchaseOrdersViewService _purchaseOrdersViewService;

        public PurchaseOrdersViewController(PurchaseOrdersViewService purchaseOrdersViewService)
        {
            _purchaseOrdersViewService = purchaseOrdersViewService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _purchaseOrdersViewService.GetAllAsync();

            return View(model);
        }
    }
}
