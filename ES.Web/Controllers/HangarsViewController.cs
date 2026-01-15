using ES.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace ES.Web.Controllers.client
{
    public class HangarsViewController : Controller
    {
        private readonly HangarsViewService _hangarsviewService;

        public HangarsViewController(HangarsViewService hangarsviewService)
        {
            _hangarsviewService = hangarsviewService;
        }

        public async Task<IActionResult> Index(int branchId)
        {
            if (branchId == 0)
            {
                return BadRequest("branchId is required");
            }

            var model = await _hangarsviewService.GetAllAsync(branchId);
            return View(model);
        }
    }
}
