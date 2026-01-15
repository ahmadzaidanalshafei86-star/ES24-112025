using ES.Web.Models;
using ES.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ES.Web.Controllers
{
    [AllowAnonymous]
    public class TenderController : Controller
    {
        private readonly TenderService _tenderService;
        private readonly TenderInquiryService _inquiryService;
        private readonly IStringLocalizer<TenderController> _localizer;

        public TenderController(
            TenderService tenderService,
            TenderInquiryService inquiryService,
            IStringLocalizer<TenderController> localizer)
        {
            _tenderService = tenderService;
            _inquiryService = inquiryService;
            _localizer = localizer;
        }

        // ---------------------------------------------------------
        // Show Tender Details
        // ---------------------------------------------------------
        [Route("Tender/{slug}")]
        [HttpGet]
        public async Task<IActionResult> Index(string slug)
        {
            var tenderModel = await _tenderService.GetTenderBySlugAsync(slug);

            if (tenderModel == null)
                return NotFound();

            return View("tenderDetails", tenderModel);
        }

        // ---------------------------------------------------------
        // Submit Inquiry
        // ---------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> SubmitInquiry(InquiryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = _localizer["Please fill all required fields."].Value; // استخدم .Value
                return RedirectToAction("Index", new { slug = model.TenderSlug });
            }

            await _inquiryService.SaveInquiryAsync(model);

            TempData["Success"] = _localizer["Your inquiry has been submitted successfully."].Value; // استخدم .Value

            // احصل على slug من الخدمة
            string? slug = await _inquiryService.GetTenderSlugAsync(model.TenderId);

            if (string.IsNullOrEmpty(slug))
                return NotFound();

            // إعادة التوجيه إلى رابط كامل باستخدام slug
            return Redirect($"/Tender/{slug}");
        }

    }
}
