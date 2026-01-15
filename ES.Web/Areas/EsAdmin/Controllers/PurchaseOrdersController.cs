using ES.Core.Entities;
using ES.Web.Areas.EsAdmin.Helpers;
using ES.Web.Areas.EsAdmin.Models;
using ES.Web.Areas.EsAdmin.Repositories;
using ES.Web.Areas.EsAdmin.Services;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MimeKit;
using static ES.Core.Consts.Permissions;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;




namespace ES.Web.Controllers
{
    [Area("EsAdmin")]
    public class PurchaseOrdersController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PurchaseOrdersRepository _PurchaseOrdersRepository;
        private readonly ILanguageService _languageService;
        private readonly IImageService _imageService;
        private readonly IFilesService _filesService;
        private readonly SlugService _slugService;
        private readonly LanguagesRepository _languagesRepository;
        private readonly PurchaseOrderFilesRepository _purchaseOrderFilesRepository;

        private readonly RowPermission _rowPermission;


        public PurchaseOrdersController(PurchaseOrdersRepository PurchaseOrdersRepository,
            LanguagesRepository languagesRepository,
            PurchaseOrderFilesRepository purchaseOrderFilesRepository,
            ILanguageService languageService,
            IImageService imageService,
            IFilesService filesService,
            SlugService slugService,
            CategoriesRepository categoriesRepository,
            DocumentsRepository documentsRepository,
            RowPermission rowPermission,
            RoleManager<IdentityRole> roleManager)
        {
            _PurchaseOrdersRepository = PurchaseOrdersRepository;
            _languagesRepository = languagesRepository;
            _purchaseOrderFilesRepository = purchaseOrderFilesRepository;
            _languageService = languageService;
            _imageService = imageService;
            _filesService = filesService;
            _slugService = slugService;
            _rowPermission = rowPermission;
            _roleManager = roleManager;
        }

        [Authorize(Permissions.PurchaseOrders.Read)]
        public async Task<IActionResult> Index()
        {
            var purchaseOrders = await _PurchaseOrdersRepository.GetAllPurchaseOrdersAsync();

            // Get distinct materials for the dropdown filter
            var materials = purchaseOrders
                  .Select(po => new {
                      Id = po.MaterialId,
                      Name = po.MaterialName
                  })
                  .Distinct()
                  .OrderBy(m => m.Name)
                  .ToList();

            ViewBag.materials = materials;

            return View(purchaseOrders);
        }

        [HttpGet]
        [Authorize(Permissions.PurchaseOrders.Create)]
        public async Task<IActionResult> Create()
        {
            var model = await _PurchaseOrdersRepository.InitializePurchaseOrderFormViewModelAsync();
            return View("Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.PurchaseOrders.Create)]
        public async Task<IActionResult> Create(PurchaseOrderFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _PurchaseOrdersRepository.InitializePurchaseOrderFormViewModelAsync();
                return View("Form", model);
            }

            PurchaseOrder purchaseOrder = new()
            {
                Title = model.Title,
                Slug = _slugService.GenerateUniqueSlug(model.Title, nameof(PurchaseOrder)),
                Code = model.Code,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                EnvelopeOpeningDate = model.EnvelopeOpeningDate,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Details = model.Details,
                MetaDescription = model.MetaDescription,
                MetaKeywords = model.MetaKeywords,
                Publish = model.Publish,
                Numberofparticipatingcompanies = model.Numberofparticipatingcompanies,
                Thenumberofcompaniesreferredto = model.Thenumberofcompaniesreferredto,
                MoveToArchive = model.MoveToArchive,
                LanguageId = await _languagesRepository.GetLanguageByCode(await _languageService.GetDefaultDbCultureAsync()),
            };

            var purchaseOrderId = await _PurchaseOrdersRepository.AddPurchaseOrderAsync(purchaseOrder);

            // Handle PurchaseOrder Image
            if (model.PurchaseOrderImage is not null)
            {
                var imageName = $"{purchaseOrderId}_PurchaseOrderImage{Path.GetExtension(model.PurchaseOrderImage.FileName)}";
                var (isUploaded, errorMessage) = await _imageService.UploadASync(model.PurchaseOrderImage, imageName, "/images/PurchaseOrders");

                if (isUploaded)
                {
                    purchaseOrder.PurchaseOrderImageUrl = imageName;
                    purchaseOrder.PurchaseOrderImageAltName = model.PurchaseOrderImage.FileName;
                    _PurchaseOrdersRepository.UpdatePurchaseOrder(purchaseOrder);
                }
                else
                {
                    ModelState.AddModelError(nameof(model.PurchaseOrderImage), errorMessage!);
                    model = await _PurchaseOrdersRepository.InitializePurchaseOrderFormViewModelAsync();
                    return View("Form", model);
                }
            }

            // Add related Materials
            if (model.SelectedMaterialIds.Any())
                await _PurchaseOrdersRepository.AddRelatedMaterialsAsync(purchaseOrder, model.SelectedMaterialIds);

          


            // Files
            if (model.PurchaseOrderFiles != null && model.PurchaseOrderFiles.Any(f => f != null && f.Length > 0))
            {
                for (int i = 0; i < model.PurchaseOrderFiles.Count; i++)
                {
                    var file = model.PurchaseOrderFiles[i];
                    if (file == null || file.Length == 0) continue;

                    PurchaseOrderFile purchaseOrderFile = new();
                    var fileName = $"{purchaseOrderId}_{i}{Path.GetExtension(file.FileName)}";

                    var (isUploaded, errorMessage) = await _filesService.UploadASync(file, fileName, "/CMS/documents/PurchaseOrders");

                    if (isUploaded)
                    {
                        purchaseOrderFile.FileUrl = fileName;
                        purchaseOrderFile.AltName = file.FileName;
                        purchaseOrderFile.PurchaseOrderId = purchaseOrder.Id;
                        purchaseOrderFile.DisplayOrder = i;

                        await _purchaseOrderFilesRepository.AddFileAsync(purchaseOrderFile);
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.PurchaseOrderFiles), errorMessage!);
                        model = await _PurchaseOrdersRepository.InitializePurchaseOrderFormViewModelAsync(model);
                        return View("Form", model);
                    }
                }
            }

            return Json(new { success = true, id = purchaseOrderId });
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var purchaseOrder = await _PurchaseOrdersRepository.GetPurchaseOrderWithGalleryImagesAsync(id);
            if (purchaseOrder is null)
                return NotFound();

            // Row Level Permission
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null)
                return NotFound();

            var RowLevelPermission = await _rowPermission
                                          .HasRowLevelPermissionAsync(role.Id,
                                          TablesNames.PurchaseOrders,
                                          purchaseOrder.Id,
                                          CrudOperations.Update);

            if (!RowLevelPermission && !(role.Name == AppRoles.SuperAdmin))
                return Redirect("/Identity/Account/AccessDenied");

            var model = new PurchaseOrderFormViewModel()
            {
                Id = purchaseOrder.Id,
                Title = purchaseOrder.Title,
                Code = purchaseOrder.Code,
                StartDate = purchaseOrder.StartDate,
                EndDate = purchaseOrder.EndDate,
                EnvelopeOpeningDate = purchaseOrder.EnvelopeOpeningDate,
                Details = purchaseOrder.Details,
                MetaDescription = purchaseOrder.MetaDescription,
                MetaKeywords = purchaseOrder.MetaKeywords,
                PurchaseOrderImageUrl = purchaseOrder.PurchaseOrderImageUrl,
                Publish = purchaseOrder.Publish,
                Numberofparticipatingcompanies = purchaseOrder.Numberofparticipatingcompanies,
                Thenumberofcompaniesreferredto = purchaseOrder.Thenumberofcompaniesreferredto,
                MoveToArchive = purchaseOrder.MoveToArchive,
                SelectedMaterialIds = purchaseOrder.Materials?.Select(c => c.MaterialId).ToList(),
                ExistingFiles = purchaseOrder.PurchaseOrderFiles?.Select(f => new PurchaseOrderFileViewModel
                {
                    FileUrl = f.FileUrl,
                    FileName = f.AltName,
                    DisplayOrder = f.DisplayOrder
                }).OrderBy(f => f.DisplayOrder)
                  .ToList()
            };

            model = await _PurchaseOrdersRepository.InitializePurchaseOrderFormViewModelAsync(model);

            return View("Form", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, PurchaseOrderFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _PurchaseOrdersRepository.InitializePurchaseOrderFormViewModelAsync(model);
                return View("Form", model);
            }

            var purchaseOrder = await _PurchaseOrdersRepository.GetPurchaseOrderByIdAsync(id);
            if (purchaseOrder is null) return NotFound();

            // --- Row Level Permission ---
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null) return NotFound();

            var hasPermission = await _rowPermission.HasRowLevelPermissionAsync(
                role.Id,
                TablesNames.PurchaseOrders,
                purchaseOrder.Id,
                CrudOperations.Update
            );

            if (!hasPermission && role.Name != AppRoles.SuperAdmin)
                return Redirect("/Identity/Account/AccessDenied");

            // --- Update purchase order properties ---
            purchaseOrder.Title = model.Title;
            purchaseOrder.Slug = _slugService.GenerateUniqueSlug(model.Title, nameof(PurchaseOrder), purchaseOrder.Id);
            purchaseOrder.Code = model.Code;
            purchaseOrder.StartDate = model.StartDate;
            purchaseOrder.EndDate = model.EndDate;
            purchaseOrder.EnvelopeOpeningDate = model.EnvelopeOpeningDate;
            purchaseOrder.CreatedDate = DateTime.Now;
            purchaseOrder.UpdatedDate = DateTime.Now;
            purchaseOrder.Details = model.Details;
            purchaseOrder.MetaDescription = model.MetaDescription;
            purchaseOrder.MetaKeywords = model.MetaKeywords;
            purchaseOrder.Publish = model.Publish;
            purchaseOrder.Numberofparticipatingcompanies = model.Numberofparticipatingcompanies;
            purchaseOrder.Thenumberofcompaniesreferredto = model.Thenumberofcompaniesreferredto;
            purchaseOrder.MoveToArchive = model.MoveToArchive;

            // --- Add related materials ---
            if (model.SelectedMaterialIds != null && model.SelectedMaterialIds.Any())
                await _PurchaseOrdersRepository.AddRelatedMaterialsAsync(purchaseOrder, model.SelectedMaterialIds);

        


            // --- Handle existing files ---
            var oldFiles = await _purchaseOrderFilesRepository.GetFilesOfPurchaseOrderAsync(purchaseOrder.Id);
            var removedFileIds = model.RemovedFiles ?? new List<int>();

            // Delete removed files
            var filesToDelete = oldFiles.Where(f => removedFileIds.Contains(f.Id)).ToList();
            foreach (var file in filesToDelete)
            {
                _purchaseOrderFilesRepository.DeleteFile(file);
                _filesService.Delete($"/documents/PurchaseOrders/{file.FileUrl}");
            }

            // Keep remaining old files
            var remainingOldFiles = oldFiles.Where(f => !removedFileIds.Contains(f.Id)).ToList();

            // --- Add new uploaded files ---
            if (model.PurchaseOrderFiles != null && model.PurchaseOrderFiles.Any(f => f != null && f.Length > 0))
            {
                int startIndex = remainingOldFiles.Count;
                for (int i = 0; i < model.PurchaseOrderFiles.Count; i++)
                {
                    var file = model.PurchaseOrderFiles[i];
                    if (file == null || file.Length == 0) continue;

                    var purchaseOrderFile = new PurchaseOrderFile();
                    var fileName = $"{purchaseOrder.Id}_{startIndex + i}{Path.GetExtension(file.FileName)}";

                    var (isUploaded, errorMessage) = await _filesService.UploadASync(file, fileName, "/CMS/documents/PurchaseOrders");
                    if (!isUploaded)
                    {
                        ModelState.AddModelError(nameof(model.PurchaseOrderFiles), errorMessage!);
                        model = await _PurchaseOrdersRepository.InitializePurchaseOrderFormViewModelAsync(model);
                        return View("Form", model);
                    }

                    purchaseOrderFile.FileUrl = fileName;
                    purchaseOrderFile.AltName = file.FileName;
                    purchaseOrderFile.PurchaseOrderId = purchaseOrder.Id;
                    purchaseOrderFile.DisplayOrder = startIndex + i;

                    await _purchaseOrderFilesRepository.AddFileAsync(purchaseOrderFile);
                }
            }

            // --- Save purchase order ---
            _PurchaseOrdersRepository.UpdatePurchaseOrder(purchaseOrder);

            return Json(new { success = true, id = purchaseOrder.Id });
        }







        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var purchaseOrder = await _PurchaseOrdersRepository.GetPurchaseOrderByIdAsync(id);

            if (purchaseOrder is null)
                return NotFound();

            // Row level Permission Check
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null)
                return NotFound();

            var RowLevelPermission = await _rowPermission
                                        .HasRowLevelPermissionAsync(role.Id,
                                        TablesNames.PurchaseOrders,
                                        purchaseOrder.Id,
                                        CrudOperations.Delete);

            if (!RowLevelPermission && !(role.Name == AppRoles.SuperAdmin))
                return StatusCode(403);

            _PurchaseOrdersRepository.DeletePurchaseOrder(purchaseOrder);

            return StatusCode(200);
        }


        [HttpPost]
        [Authorize(Permissions.PurchaseOrders.Update)]
        public async Task<IActionResult> ToggleArchive(int id)
        {
            var purchaseOrder = await _PurchaseOrdersRepository.GetPurchaseOrderByIdAsync(id);
            if (purchaseOrder == null) return NotFound();

            // Row-level permission check (similar to ToggleStatus)
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role == null) return Forbid();

            var hasPermission = await _rowPermission.HasRowLevelPermissionAsync(
                role.Id, TablesNames.PurchaseOrders, purchaseOrder.Id, CrudOperations.Update
            );

            if (!hasPermission && role.Name != AppRoles.SuperAdmin)
                return Forbid();

            purchaseOrder.MoveToArchive = !purchaseOrder.MoveToArchive;
            _PurchaseOrdersRepository.UpdatePurchaseOrder(purchaseOrder);

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var purchaseOrder = await _PurchaseOrdersRepository.GetPurchaseOrderByIdAsync(id);

            if (purchaseOrder is null)
                return StatusCode(402);

            // Row level Permission Check
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null)
                return NotFound();

            var RowLevelPermission = await _rowPermission
                                       .HasRowLevelPermissionAsync(role.Id,
                                       TablesNames.PurchaseOrders,
                                       purchaseOrder.Id,
                                       CrudOperations.Update);

            if (!RowLevelPermission && !(role.Name == AppRoles.SuperAdmin))
                return StatusCode(403);

            purchaseOrder.Publish = !purchaseOrder.Publish;
            _PurchaseOrdersRepository.UpdatePurchaseOrder(purchaseOrder);

            return StatusCode(200);
        }



    }
}
