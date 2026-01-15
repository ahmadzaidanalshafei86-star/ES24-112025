using ES.Web.Models;
using ES.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ES.Web.Controllers
{
    [AllowAnonymous]
    public class PurchaseOrderController : Controller
    {
        private readonly PurchaseOrderService _purchaseOrderService;
        private readonly PurchaseOrderInquiryService _inquiryService;
        private readonly IStringLocalizer<PurchaseOrderController> _localizer;

        public PurchaseOrderController(
            PurchaseOrderService purchaseOrderService,
            PurchaseOrderInquiryService inquiryService,
            IStringLocalizer<PurchaseOrderController> localizer)
        {
            _purchaseOrderService = purchaseOrderService;
            _inquiryService = inquiryService;
            _localizer = localizer;
        }

        // ---------------------------------------------------------
        // Show Purchase Order Details
        // ---------------------------------------------------------
        [Route("PurchaseOrder/{slug}")]
        [HttpGet]
        public async Task<IActionResult> Index(string slug)
        {
            var poModel = await _purchaseOrderService.GetPurchaseOrderBySlugAsync(slug);

            if (poModel == null)
                return NotFound();

            return View("purchaseOrderDetails", poModel);
        }

        // ---------------------------------------------------------
        // Submit Inquiry
        // ---------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> SubmitInquiry(PurchaseOrderInquiryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = _localizer["Please fill all required fields."].Value;
                return RedirectToAction("Index", new { slug = model.PurchaseOrderSlug });
            }

            await _inquiryService.SaveInquiryAsync(model);

            TempData["Success"] = _localizer["Your inquiry has been submitted successfully."].Value;

            // Get slug
            string? slug = await _inquiryService.GetPurchaseOrderSlugAsync(model.PurchaseOrderId);

            if (string.IsNullOrEmpty(slug))
                return NotFound();

            // Redirect using slug
            return Redirect($"/PurchaseOrder/{slug}");
        }
    }
}
