


using ES.Web.Services;

namespace ES.Web.Controllers.client
{
    public class TendersArchiveViewController : Controller
    {
        private readonly TendersArchiveViewService _tendersArchiveviewService;

        public TendersArchiveViewController(TendersArchiveViewService tendersArchiveviewService)
        {
            _tendersArchiveviewService = tendersArchiveviewService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _tendersArchiveviewService.GetAllAsync();

            return View(model);
        }
    }
}

