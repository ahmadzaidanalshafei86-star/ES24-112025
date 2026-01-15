


using ES.Web.Services;

namespace ES.Web.Controllers.client
{
    public class TendersViewController : Controller
    {
        private readonly TendersViewService _tendersviewService;

        public TendersViewController(TendersViewService tendersviewService)
        {
            _tendersviewService = tendersviewService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _tendersviewService.GetAllAsync();

            return View(model);
        }
    }
}

