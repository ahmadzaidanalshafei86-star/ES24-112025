



using ES.Core.Entities;
using ES.Web.Areas.EsAdmin.Helpers;
using ES.Web.Areas.EsAdmin.Models;
using ES.Web.Areas.EsAdmin.Repositories;
using ES.Web.Areas.EsAdmin.Services;

namespace ES.Web.Controllers
{
    [Area("EsAdmin")]
    public class BranchesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IImageService _imageService;
        private readonly SlugService _slugService;
        private readonly ILanguageService _languageService;
        private readonly RowPermission _rowPermission;
        private readonly BranchesRepository _BranchesRepository;
        private readonly LanguagesRepository _languagesRepository;
        private readonly GalleryImagesRepository _galleryImagesRepository;
        private readonly MenuItemsRepository _menuItemsRepository;

        public BranchesController(LanguagesRepository languagesRepository,
            SlugService slugService,
            BranchesRepository BranchesRepository,
            GalleryImagesRepository galleryImagesRepository,
            IImageService imageService,
            ILanguageService languageService,
            RowPermission rowPermission,
            RoleManager<IdentityRole> roleManager,
            MenuItemsRepository menuItemsRepository)
        {
            _galleryImagesRepository = galleryImagesRepository;
            _languagesRepository = languagesRepository;
            _imageService = imageService;
            _slugService = slugService;
            _languageService = languageService;
            _BranchesRepository = BranchesRepository;
            _rowPermission = rowPermission;
            _roleManager = roleManager;
            _menuItemsRepository = menuItemsRepository;
        }

        [Authorize(Permissions.Branches.Read)]
        public async Task<IActionResult> Index()
        {
            var Branches = await _BranchesRepository.GetBranchesWithParentInfoAsync();
            return View(Branches);
        }

        [HttpGet]
        [Authorize(Permissions.Branches.Create)]
        public async Task<IActionResult> Create()
        {
            var model = await _BranchesRepository.InitializebranchFormViewModelAsync();
            return View("Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Branches.Create)]
        public async Task<IActionResult> Create(BranchFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _BranchesRepository.InitializebranchFormViewModelAsync();
                return View("Form", model);
            }

            Branch branch = new()
            {
                Name = model.Name,
                Slug = _slugService.GenerateUniqueSlug(model.Name, nameof(Branch)),
                LanguageId = await _languagesRepository.GetLanguageByCode(await _languageService.GetDefaultDbCultureAsync())
            };

            // Save Branch
            var branchId = await _BranchesRepository.AddbranchAsync(branch);

            // -------------------------------
            // 🔥 Add HANGARS
            // -------------------------------
            if (model.Hangars != null && model.Hangars.Count > 0)
            {
                foreach (var h in model.Hangars)
                {
                    var hangar = new Hangar
                    {
                        Name = h.Name,
                        Size = h.Size,
                        Type = h.Type,
                        BranchId = branchId
                    };

                    await _BranchesRepository.AddHangarAsync(hangar);
                }
            }

            // -------------------------------
            // 🔥 Add REFRIGERATORS
            // -------------------------------
            if (model.Refrigators != null && model.Refrigators.Count > 0)
            {
                foreach (var r in model.Refrigators)
                {
                    var refrigator = new Refrigator
                    {
                        Name = r.Name,
                        Size = r.Size,
                        Type = r.Type,
                        BranchId = branchId
                    };

                    await _BranchesRepository.AddRefrigatorAsync(refrigator);
                }
            }

            return Json(new { success = true, id = branchId });
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var branch = await _BranchesRepository.GetbranchWithAllDataAsync(id);

            if (branch is null)
                return NotFound();

            //Row level Permission Check
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null)
                return NotFound();
            var RowLevelPermission = await _rowPermission
                                           .HasRowLevelPermissionAsync(role.Id,
                                           TablesNames.Branches,
                                           branch.Id,
                                           CrudOperations.Update);

            if (!RowLevelPermission && !(role.Name == AppRoles.SuperAdmin))
                return Redirect("/Identity/Account/AccessDenied");


            var model = new BranchFormViewModel
            {
                Id = branch.Id,
                Name = branch.Name
            };

            model = await _BranchesRepository.InitializebranchFormViewModelAsync(model);

            return View("Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, BranchFormViewModel model)
        {
            // ===============================
            // ✅ تحقق من صلاحية المستخدم على مستوى الصف
            // ===============================
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null) return NotFound();

            var hasPermission = await _rowPermission
                .HasRowLevelPermissionAsync(role.Id, TablesNames.Branches, id, CrudOperations.Update);

            if (!hasPermission && role.Name != AppRoles.SuperAdmin)
                return Redirect("/Identity/Account/AccessDenied");

            // ===============================
            // ✅ تحقق من صحة النموذج
            // ===============================
            if (!ModelState.IsValid)
            {
                model = await _BranchesRepository.InitializebranchFormViewModelAsync(model);
                return View("Form", model);
            }

            // ===============================
            // ✅ جلب الفرع مع البيانات المرتبطة
            // ===============================
            var branch = await _BranchesRepository.GetbranchWithAllDataAsync(id);
            if (branch == null) return NotFound();

            // ===============================
            // ✅ تحديث الاسم و slug إذا تغير
            // ===============================
            if (branch.Name != model.Name)
                branch.Slug = _slugService.GenerateUniqueSlug(model.Name, nameof(Branch), branch.Id);

            branch.Name = model.Name;
            _BranchesRepository.Updatebranch(branch);

            // ===============================
            // ✅ Debug: تحقق من الـ Hangars والـ Translates
            // ===============================
            foreach (var h in model.Hangars)
            {
                Console.WriteLine($"Hangar Name={h.Name}, Translates.Count={h.Translates.Count}");
                foreach (var ht in h.Translates)
                {
                    Console.WriteLine($"  LangId={ht.LanguageId}, Name={ht.Name}, Size={ht.Size}, Type={ht.Type}");
                }
            }

            // ===============================
            // ✅ تحديث Hangars مع الترجمة
            // ===============================
            await _BranchesRepository.DeleteHangarsByBranchIdAsync(branch.Id);

            if (model.Hangars != null)
            {
                foreach (var h in model.Hangars)
                {
                    var hangar = new Hangar
                    {
                        Name = h.Name,
                        Size = h.Size,
                        Type = h.Type,
                        BranchId = branch.Id
                    };
                    await _BranchesRepository.AddHangarAsync(hangar);

                    // إضافة الترجمة فقط إذا تحتوي على قيمة
                    if (h.Translates != null)
                    {
                        foreach (var ht in h.Translates
                                     .Where(t => !string.IsNullOrWhiteSpace(t.Name) ||
                                                 !string.IsNullOrWhiteSpace(t.Size) ||
                                                 !string.IsNullOrWhiteSpace(t.Type)))
                        {
                            await _BranchesRepository.AddHangarTranslationAsync(hangar.Id, ht);
                        }
                    }
                }
            }

            // ===============================
            // ✅ تحديث Refrigators مع الترجمة
            // ===============================
            await _BranchesRepository.DeleteRefrigatorsByBranchIdAsync(branch.Id);

            if (model.Refrigators != null)
            {
                foreach (var r in model.Refrigators)
                {
                    var refrigator = new Refrigator
                    {
                        Name = r.Name,
                        Size = r.Size,
                        Type = r.Type,
                        BranchId = branch.Id
                    };
                    await _BranchesRepository.AddRefrigatorAsync(refrigator);

                    if (r.Translates != null)
                    {
                        foreach (var rt in r.Translates
                                     .Where(t => !string.IsNullOrWhiteSpace(t.Name) ||
                                                 !string.IsNullOrWhiteSpace(t.Size) ||
                                                 !string.IsNullOrWhiteSpace(t.Type)))
                        {
                            await _BranchesRepository.AddRefrigatorTranslationAsync(refrigator.Id, rt);
                        }
                    }
                }
            }

            // ===============================
            // ✅ إعادة JSON نجاح العملية
            // ===============================
            return Json(new { success = true, id = branch.Id });
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            var branch = await _BranchesRepository.GetBranchByIdAsync(id);

            if (branch is null)
                return NotFound();

            // Prevent Deleting "Home Slider"
            if (branch.Name == "Home Slider")
                return BadRequest("This branch cannot be deleted.");

            //Check Authroization to delete this branch
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null)
                return NotFound();
            var RowLevelPermission = await _rowPermission
                .HasRowLevelPermissionAsync(role.Id, TablesNames.Branches, id, CrudOperations.Delete);
            if (!RowLevelPermission && !(role.Name == AppRoles.SuperAdmin))
                return StatusCode(403);

            //Remove all row level permissions of this branch
            var existingbranchRowPermissions = await _rowPermission
                .GetRowLevelPermissionsToDeleteAsync(id, TablesNames.Branches);
            _rowPermission.RemoveRange(existingbranchRowPermissions);

            //Remove all row level permissions of related pages
            var existingPageRowPermissions = await _rowPermission
                .GetRowLevelPermissionsToDeleteAsync(id, TablesNames.Branches);
            _rowPermission.RemoveRange(existingPageRowPermissions);

            //if (_BranchesRepository.IsParentbranch(id) || _BranchesRepository.IsRelatedToAnotherbranch(id))
            //    return StatusCode(400);






            var result = await _BranchesRepository.DeletebranchAsync(id); //returns true if deleted successfully
            if (result)
                return StatusCode(200);

            return BadRequest();

        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            //Check Authroization to update this branch 
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null)
                return NotFound();
            var RowLevelPermission = await _rowPermission
                                          .HasRowLevelPermissionAsync(role.Id,
                                          TablesNames.Branches,
                                          id,
                                          CrudOperations.Update);

            if (!RowLevelPermission && !(role.Name == AppRoles.SuperAdmin))
                return StatusCode(403);

            var branch = await _BranchesRepository.GetBranchByIdAsync(id);

            if (branch is null)
                return NotFound();



            _BranchesRepository.Updatebranch(branch);

            return StatusCode(200);
        }
    }
}


