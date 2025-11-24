using AKM.Core.Entities;
using ES.Web.Areas.EsAdmin.Helpers;
using ES.Web.Areas.EsAdmin.Models;
using ES.Web.Areas.EsAdmin.Repositories;

namespace ES.Web.Controllers
{
    [Area("EsAdmin")]
    public class TendersTranslatesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TenderTranslatesRepository _tenderTranslatesRepository;
        private readonly TendersRepository _tendersRepository;
        private readonly RowPermission _rowPermission;

        public TendersTranslatesController(
            TendersRepository tendersRepository,
            TenderTranslatesRepository tenderTranslatesRepository,
            RoleManager<IdentityRole> roleManager,
            RowPermission rowPermission)
        {
            _tendersRepository = tendersRepository;
            _tenderTranslatesRepository = tenderTranslatesRepository;
            _roleManager = roleManager;
            _rowPermission = rowPermission;
        }

        // ------------------ Index ------------------
        [Authorize(Permissions.Tenders.Read)]
        public async Task<IActionResult> Index(int tenderId)
        {
            var tender = await _tendersRepository.GetTenderByIdWithTranslationsAsync(tenderId);

            TenderTranslatesViewModel model = new()
            {
                TenderId = tenderId,
                TenderTitle = tender.Title,
                TenderDefaultLang = tender.Language!.Code,
                CreatedDate = tender.CreatedDate,
                PreEnteredTranslations = tender.TenderTranslates?.ToList(),
            };
            return View(model);
        }

        // ------------------ Create GET ------------------
        [HttpGet]
        [Authorize(Permissions.Tenders.Create)]
        public async Task<IActionResult> Create(int tenderId)
        {
            var model = await _tenderTranslatesRepository.InitializeTenderTranslatesFormViewModelAsync(tenderId);
            model.TenderId = tenderId;

            var tender = await _tendersRepository.GetTenderByIdAsync(tenderId);

            model.Title = tender.Title;
            model.Details = tender.Details;
            model.PricesOffered = tender.PricesOffered;
            model.MetaDescription = tender.MetaDescription;
            model.MetaKeywords = tender.MetaKeywords;

            return View("Form", model);
        }

        // ------------------ Create POST ------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Tenders.Create)]
        public async Task<IActionResult> Create(TenderTranslationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _tenderTranslatesRepository.InitializeTenderTranslatesFormViewModelAsync(model.TenderId);
                return View("Form", model);
            }

            TenderTranslate tenderTranslate = new()
            {
                Title = model.Title,
                Details = model.Details,
                PricesOffered = model.PricesOffered,
                MetaDescription = model.MetaDescription,
                MetaKeywords = model.MetaKeywords,
                CreatedDate = DateTime.UtcNow,
                LanguageId = (int)model.LanguageId,
                TenderId = model.TenderId,
            };

            await _tenderTranslatesRepository.AddTenderTranslateAsync(tenderTranslate);

            return RedirectToAction("Index", new { tenderId = model.TenderId });
        }

        // ------------------ Edit GET ------------------
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int tenderId, int translateId)
        {
            var translate = await _tenderTranslatesRepository.GetTenderTranslateByIdAsync(translateId);

            // Permission check
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null)
                return NotFound();

            var RowLevelPermission = await _rowPermission.HasRowLevelPermissionAsync(role.Id, TablesNames.Tenders, tenderId, CrudOperations.Update);

            if (!RowLevelPermission && !(role.Name == AppRoles.SuperAdmin))
                return Redirect("/Identity/Account/AccessDenied");

            TenderTranslationFormViewModel model = new()
            {
                TranslationId = translateId,
                Title = translate.Title,
                Details = translate.Details,
                PricesOffered = translate.PricesOffered,
                MetaDescription = translate.MetaDescription,
                MetaKeywords = translate.MetaKeywords,
            };

            model = await _tenderTranslatesRepository.InitializeTenderTranslatesFormViewModelAsync(tenderId, model);
            model.TenderId = tenderId;

            return View("Form", model);
        }

        // ------------------ Edit POST ------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(TenderTranslationFormViewModel model, int tenderId)
        {
            if (!ModelState.IsValid)
            {
                model = await _tenderTranslatesRepository.InitializeTenderTranslatesFormViewModelAsync(tenderId, model);
                model.TenderId = tenderId;
                return View("Form", model);
            }

            var translate = await _tenderTranslatesRepository.GetTenderTranslateByIdAsync(model.TranslationId);

            // Permission check
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null)
                return NotFound();

            //var RowLevelPermission = await _rowPermission.HasRowLevelPermissionAsync(
            //    role.Id,
            //    TablesNames.TendersOfCategory,
            //    translate.Tender.CategoryId,
            //    CrudOperations.Update);


            var RowLevelPermission = await _rowPermission.HasRowLevelPermissionAsync(role.Id, TablesNames.Tenders, tenderId, CrudOperations.Update);

            if (!RowLevelPermission && !(role.Name == AppRoles.SuperAdmin))
                return Redirect("/Identity/Account/AccessDenied");

            translate.Title = model.Title;
            translate.Details = model.Details;
            translate.PricesOffered = model.PricesOffered;
            translate.MetaDescription = model.MetaDescription;
            translate.MetaKeywords = model.MetaKeywords;

            _tenderTranslatesRepository.UpdateTenderTranslate(translate);

            return RedirectToAction("Index", new { tenderId = model.TenderId });
        }

        // ------------------ Delete ------------------
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var translate = await _tenderTranslatesRepository.GetTenderTranslateByIdAsync(id);

            if (translate == null)
                return StatusCode(404);

            // Permission check
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null)
                return NotFound();

            var RowLevelPermission = await _rowPermission.HasRowLevelPermissionAsync(role.Id, TablesNames.Tenders, id, CrudOperations.Delete);

            if (!RowLevelPermission && !(role.Name == AppRoles.SuperAdmin))
                return StatusCode(403);

            var result = await _tenderTranslatesRepository.DeleteTranslationAsync(id);

            if (result)
                return StatusCode(200);

            return BadRequest();
        }
    }
}
