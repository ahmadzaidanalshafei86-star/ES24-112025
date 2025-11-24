using ES.Web.Models;
using ES.Web.Services;
using Microsoft.Extensions.Localization;

namespace ES.Web.Controllers
{
    public class SuppliersFormController : Controller
    {
        private readonly SuppliersService _suppliersService;
        private readonly IStringLocalizer<SuppliersFormController> _localizer;

        public SuppliersFormController(SuppliersService suppliersService, IStringLocalizer<SuppliersFormController> localizer)
        {
            _suppliersService = suppliersService;
            _localizer = localizer;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _suppliersService.InitializeSupplierFormViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(SupplierFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var refreshedModel = await _suppliersService.InitializeSupplierFormViewModel();
                // Preserve user input
                refreshedModel.UserType = model.UserType;
                refreshedModel.CompanyName = model.CompanyName;
                refreshedModel.CompanySector = model.CompanySector;
                refreshedModel.RegistrationNumber = model.RegistrationNumber;
                refreshedModel.PhoneNumber = model.PhoneNumber;
                refreshedModel.EmailAddress = model.EmailAddress;
                refreshedModel.FaxNumber = model.FaxNumber;
                refreshedModel.WebsiteUrl = model.WebsiteUrl;
                refreshedModel.Address = model.Address;

                return View("Index", refreshedModel);
            }

            // Get selected materials
            var selectedMaterials = model.Materials
                .Where(m => m.IsSelected)
                .Select(m => new { m.Id, m.Name })
                .ToList();

            // TODO: Save selected materials to database
            // For now, just storing in a variable for debugging
            var materialsToSave = selectedMaterials;

            // TODO: Save the supplier form data to database
            // For now, just return success message
            TempData["SuccessMessage"] = _localizer["Your supplier application has been submitted successfully!"].Value;
            return RedirectToAction("Index");
        }
    }
}
