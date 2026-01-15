using ES.Web.Models;
using ES.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ES.Web.Controllers
{
    public class BookServiceController : Controller
    {
        private readonly BookServiceService _bookserviceService;
        private readonly IStringLocalizer<BookServiceController> _localizer;

        public BookServiceController(BookServiceService bookserviceService, IStringLocalizer<BookServiceController> localizer)
        {
            _bookserviceService = bookserviceService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _bookserviceService.InitializeBookServiceFormViewModel();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetBranchDetails(int branchId)
        {
            var branch = await _bookserviceService.GetBranchWithDetailsAsync(branchId);

            if (branch == null)
                return NotFound();

            // Hangars
            var hangars = branch.RelatedHangars?
                .Select(h => new HangarCheckboxViewModel
                {
                    Id = h.Id,
                    Name = $"{h.Name} | {h.Size} | {h.Type}",
                    IsSelected = false
                })
                .ToList() ?? new List<HangarCheckboxViewModel>();

            // Refrigators
            var refrigators = branch.RelatedRefrigators?
                .Select(r => new RefrigatorCheckboxViewModel
                {
                    Id = r.Id,
                    Name = $"{r.Name} | {r.Size} | {r.Type}",
                    IsSelected = false
                })
                .ToList() ?? new List<RefrigatorCheckboxViewModel>();

            return Json(new { hangars, refrigators });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(BookServiceFormViewModel model)
        {
            var refreshedModel = await _bookserviceService.InitializeBookServiceFormViewModel();
            refreshedModel.Name = model.Name;
            refreshedModel.PhoneNumber = model.PhoneNumber;
            refreshedModel.EmailAddress = model.EmailAddress;
            refreshedModel.Notes = model.Notes;
            refreshedModel.BranchId = model.BranchId;

            try
            {
                if (!model.BranchId.HasValue)
                {
                    TempData["ErrorMessage"] = "Please select a branch.";
                    return View("Index", refreshedModel);
                }

                var request = new BookServiceRequest
                {
                    Name = model.Name ?? string.Empty,
                    PhoneNumber = model.PhoneNumber ?? string.Empty,
                    EmailAddress = model.EmailAddress,
                    Notes = model.Notes,
                    BranchId = model.BranchId.Value,
                    CreatedAt = DateTime.UtcNow
                };

                // إضافة Hangars إذا تم اختيارها
                if (model.Hangars != null && model.Hangars.Any(h => h.IsSelected))
                {
                    request.Hangars = model.Hangars
                        .Where(h => h.IsSelected)
                        .Select(h => new BookServiceHangar
                        {
                            HangarId = h.Id,
                            Name = h.Name,
                            BookServiceRequest = request
                        })
                        .ToList();
                }

                // إضافة Refrigators إذا تم اختيارها
                if (model.Refrigators != null && model.Refrigators.Any(r => r.IsSelected))
                {
                    request.Refrigators = model.Refrigators
                        .Where(r => r.IsSelected)
                        .Select(r => new BookServiceRefrigator
                        {
                            RefrigatorId = r.Id,
                            Name = r.Name,
                            BookServiceRequest = request
                        })
                        .ToList();
                }

                // حفظ الطلب في قاعدة البيانات
                await _bookserviceService.SaveBookServiceRequestAsync(request);

                TempData["SuccessMessage"] = _localizer["Your supplier application has been submitted successfully!"].Value;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = _localizer[$"An error occurred: {ex.Message}"].Value;
                return View("Index", refreshedModel);
            }
        }


    }

}
