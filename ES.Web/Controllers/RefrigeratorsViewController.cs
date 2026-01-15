

using ES.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace ES.Web.Controllers.client
{
    public class RefrigeratorsViewController : Controller
    {
        private readonly RefrigeratorsViewService _refrigeratorsviewService;

        public RefrigeratorsViewController(RefrigeratorsViewService refrigeratorsviewService)
        {
            _refrigeratorsviewService = refrigeratorsviewService;
        }

        public async Task<IActionResult> Index(int branchId)
        {
            if (branchId == 0)
            {
                return BadRequest("branchId is required");
            }

            var model = await _refrigeratorsviewService.GetAllAsync(branchId);
            return View(model);
        }
    }
}
