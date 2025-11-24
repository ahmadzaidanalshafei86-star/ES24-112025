


using ES.Web.Areas.EsAdmin.Helpers;
using ES.Web.Areas.EsAdmin.Models;
using ES.Web.Areas.EsAdmin.Repositories;

namespace ES.Web.Controllers
{
    [Area("EsAdmin")]
    public class BranchesTranslatesController : Controller
    {
        private readonly BranchesRepository _BranchesRepository;
        private readonly BranchesTranslatesRepository _BranchesTranslatesRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RowPermission _rowPermission;
        public BranchesTranslatesController(BranchesRepository BranchesRepository,
            BranchesTranslatesRepository BranchesTranslatesRepository,
            RowPermission rowPermission,
            RoleManager<IdentityRole> roleManager)
        {
            _BranchesRepository = BranchesRepository;
            _BranchesTranslatesRepository = BranchesTranslatesRepository;
            _rowPermission = rowPermission;
            _roleManager = roleManager;
        }

        [Authorize(Permissions.Branches.Read)]
        public async Task<IActionResult> Index(int branchId)
        {
            var branch = await _BranchesRepository.GetBranchByIdWithTranslationsAsync(branchId);

            BranchTranslatesViewModel model = new()
            {
                BranchId = branchId,
                BranchName = branch.Name,
                BranchDefaultLang = branch.Language.Code,
                PreEnteredTranslations = branch.BranchesTranslates?.ToList(),
            };
            return View(model);
        }


        [HttpGet]
        [Authorize(Permissions.Branches.Create)]
        public async Task<IActionResult> Create(int branchId)
        {
            var model = await _BranchesTranslatesRepository.InitializeBranchTranslatesFormViewModelAsync(branchId);
            model.BranchId = branchId;

            var branch = await _BranchesRepository.GetBranchByIdAsync(branchId);

            model.Name = branch.Name;


            return View("Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Branches.Create)]
        public async Task<IActionResult> Create(BranchTranslationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _BranchesTranslatesRepository.InitializeBranchTranslatesFormViewModelAsync(model.BranchId);
                return View("Form", model);
            }

            BranchTranslate BranchTranslate = new()
            {
                Name = model.Name,
                LanguageId = (int)model.LanguageId,
                BranchId = model.BranchId,
            };

            await _BranchesTranslatesRepository.AddBranchTranslateAsync(BranchTranslate);

            return RedirectToAction("Index", new { branchId = model.BranchId });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int branchId, int Translateid)
        {
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null)
                return NotFound();

            var RowLevelPermission = await _rowPermission
                                            .HasRowLevelPermissionAsync(role.Id,
                                            TablesNames.Branches,
                                           branchId,
                                            CrudOperations.Update);

            if (!RowLevelPermission && !(role.Name == AppRoles.SuperAdmin))
                return Redirect("/Identity/Account/AccessDenied");

            var translate = await _BranchesTranslatesRepository.GetBranchTranslateByIdAsync(Translateid);

            BranchTranslationFormViewModel model = new()
            {
                TranslationId = Translateid,
                Name = translate.Name,
            };

            model = await _BranchesTranslatesRepository.InitializeBranchTranslatesFormViewModelAsync(branchId, model);
            model.BranchId = branchId;

            return View("Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(BranchTranslationFormViewModel model, int branchId)
        {

            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null)
                return NotFound();

            var RowLevelPermission = await _rowPermission.HasRowLevelPermissionAsync(role.Id, TablesNames.Branches, branchId, CrudOperations.Update);

            if (!RowLevelPermission && !(role.Name == AppRoles.SuperAdmin))
                return Redirect("/Identity/Account/AccessDenied");

            if (!ModelState.IsValid)
            {
                model = await _BranchesTranslatesRepository.InitializeBranchTranslatesFormViewModelAsync(branchId, model);
                model.BranchId = branchId;
                return View("Form", model);
            }

            var translate = await _BranchesTranslatesRepository.GetBranchTranslateByIdAsync(model.TranslationId);

            translate.Name = model.Name;


            _BranchesTranslatesRepository.UpdateBranch(translate);

            return RedirectToAction("Index", new { branchId = model.BranchId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int translationId, int branchId)
        {
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null)
                return NotFound();

            var RowLevelPermission = await _rowPermission.HasRowLevelPermissionAsync(role.Id, TablesNames.Branches, branchId, CrudOperations.Delete);

            if (!RowLevelPermission && !(role.Name == AppRoles.SuperAdmin))
                return StatusCode(403);

            var result = await _BranchesTranslatesRepository.DeleteTranslationAsync(translationId); //returns true if deleted successfully
            if (result)
                return StatusCode(200);

            return BadRequest();

        }
    }
}
