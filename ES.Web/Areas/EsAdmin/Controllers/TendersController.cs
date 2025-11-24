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
    public class TendersController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TendersRepository _TendersRepository;
        private readonly ILanguageService _languageService;
        private readonly IImageService _imageService;
        private readonly IFilesService _filesService;
        private readonly SlugService _slugService;
        private readonly LanguagesRepository _languagesRepository;
        private readonly TenderFilesRepository _tenderFilesRepository;

        private readonly RowPermission _rowPermission;
     
        public TendersController(TendersRepository TendersRepository,
            LanguagesRepository languagesRepository,
           TenderFilesRepository tenderFilesRepository,
            ILanguageService languageService,
            IImageService imageService,
            IFilesService filesService,
            SlugService slugService,
            CategoriesRepository categoriesRepository,
            DocumentsRepository documentsRepository,
            RowPermission rowPermission,
            RoleManager<IdentityRole> roleManager)
        {
            _TendersRepository = TendersRepository;
            _languagesRepository = languagesRepository;
            _tenderFilesRepository= tenderFilesRepository;
            _languageService = languageService;
            _imageService = imageService;
            _filesService = filesService;
            _slugService = slugService;
            _rowPermission = rowPermission;
            _roleManager = roleManager;
            
        }

        [Authorize(Permissions.Tenders.Read)]
        public async Task<IActionResult> Index()
        {
            var Tenders = await _TendersRepository.GetAllTendersAsync();

            // Get distinct materials for the dropdown filter
            var materials = Tenders
                  .Select(t => new {
                      Id = t.MaterialId,
                      Name = t.MaterialName
                  })
                  .Distinct()
                  .OrderBy(m => m.Name)
                  .ToList();


            ViewBag.materials = materials;

            return View(Tenders);
        }
        [HttpGet]
        [Authorize(Permissions.Tenders.Create)]
        public async Task<IActionResult> Create()
        {
            
            var model = await _TendersRepository.InitializeTenderFormViewModelAsync();
            return View("Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.Tenders.Create)]
        public async Task<IActionResult> Create(TenderFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _TendersRepository.InitializeTenderFormViewModelAsync();
                return View("Form", model);
            }

            Tender tender = new()
            {
                Title = model.Title,
                Slug = _slugService.GenerateUniqueSlug(model.Title, nameof(Tender)),
                Code = model.Code,
                CopyPrice = model.CopyPrice,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                EnvelopeOpeningDate = model.EnvelopeOpeningDate,
                LastCopyPurchaseDate = model.LastCopyPurchaseDate,
                CreatedDate = DateTime.Now,
                Details = model.Details,
                PricesOffered = model.PricesOffered,
                MetaDescription = model.MetaDescription,
                MetaKeywords = model.MetaKeywords,
                //TenderImageUrl = model.TenderImageUrl,
                //PricesOfferedAttachmentUrl = model.PricesOfferedAttachmentUrl,
                //InitialAwardFileUrl = model.InitialAwardFileUrl,
                //FinalAwardFileUrl = model.FinalAwardFileUrl,
                Publish = model.Publish,
                PublishPricesOffered = model.PublishPricesOffered,
                SpecialOfferBlink = model.SpecialOfferBlink,
                MoveToArchive = model.MoveToArchive,
                BlinkStartDate = model.BlinkStartDate,
                BlinkEndDate = model.BlinkEndDate,
                LanguageId = await _languagesRepository.GetLanguageByCode(await _languageService.GetDefaultDbCultureAsync()),
            };

         


            var tenderId = await _TendersRepository.AddTenderAsync(tender);
            // Handle TenderImage Image
            if (model.TenderImage is not null)
            {
                var TenderImageName = $"{tenderId}_TenderImage{Path.GetExtension(model.TenderImage.FileName)}";

                var (isUploaded, errorMessage) = await _imageService.UploadASync(model.TenderImage, TenderImageName, "/images/Tenders");
                if (isUploaded)
                {
                    tender.TenderImageUrl = TenderImageName;
                    tender.TenderImageAltName = model.TenderImage.FileName;
                    _TendersRepository.Updatetender(tender);
                }
                else
                {
                    ModelState.AddModelError(nameof(model.TenderImage), errorMessage!);
                    model = await _TendersRepository.InitializeTenderFormViewModelAsync();
                    return View("Form", model);
                }
            }

            //Add related Materials
            if (model.SelectedMaterialIds.Any())
                await _TendersRepository.AddRelatedMaterialsAsync(tender, model.SelectedMaterialIds);



            // ========== Initial Award File ==========
            if (model.InitialAwardFile is not null)
            {
                var fileName = $"{tenderId}_InitialAward{Path.GetExtension(model.InitialAwardFile.FileName)}";

                var (isUploaded, errorMessage) = await _filesService.UploadASync(
                    model.InitialAwardFile,
                    fileName,
                    "/images/Tenders"
                );

                if (isUploaded)
                {
                    tender.InitialAwardFileUrl = fileName;
                    _TendersRepository.Updatetender(tender);
                }
                else
                {
                    ModelState.AddModelError(nameof(model.InitialAwardFile), errorMessage!);
                    model = await _TendersRepository.InitializeTenderFormViewModelAsync(model);
                    return View("Form", model);
                }
            }

            // ========== Final Award File ==========
            if (model.FinalAwardFile is not null)
            {
                var fileName = $"{tenderId}_FinalAward{Path.GetExtension(model.FinalAwardFile.FileName)}";

                var (isUploaded, errorMessage) = await _filesService.UploadASync(
                    model.FinalAwardFile,
                    fileName,
                    "/images/Tenders"
                );

                if (isUploaded)
                {
                    tender.FinalAwardFileUrl = fileName;
                    _TendersRepository.Updatetender(tender);
                }
                else
                {
                    ModelState.AddModelError(nameof(model.FinalAwardFile), errorMessage!);
                    model = await _TendersRepository.InitializeTenderFormViewModelAsync(model);
                    return View("Form", model);
                }
            }

            // ========== Prices Offered Attachment ==========
            if (model.PricesOfferedAttachment is not null)
            {
                var fileName = $"{tenderId}_PricesOffered{Path.GetExtension(model.PricesOfferedAttachment.FileName)}";

                var (isUploaded, errorMessage) = await _filesService.UploadASync(
                    model.PricesOfferedAttachment,
                    fileName,
                    "/images/Tenders"
                );

                if (isUploaded)
                {
                    tender.PricesOfferedAttachmentUrl = fileName;
                    _TendersRepository.Updatetender(tender);
                }
                else
                {
                    ModelState.AddModelError(nameof(model.PricesOfferedAttachment), errorMessage!);
                    model = await _TendersRepository.InitializeTenderFormViewModelAsync(model);
                    return View("Form", model);
                }
            }


            //Files
            // Files
            if (model.TenderFiles != null && model.TenderFiles.Any(f => f != null && f.Length > 0))
            {
                for (int i = 0; i < model.TenderFiles.Count; i++)
                {
                    var file = model.TenderFiles[i];

                    // Skip null or empty file inputs
                    if (file == null || file.Length == 0)
                        continue;

                    TenderFile tenderFile = new();
                    var tenderFileName = $"{tenderId}_{i}{Path.GetExtension(file.FileName)}";

                    var (isUploaded, errorMessage) = await _filesService.UploadASync(
                        file,
                        tenderFileName,
                        "/CMS/documents/Tenders"
                    );

                    if (isUploaded)
                    {
                        tenderFile.FileUrl = tenderFileName;
                        tenderFile.AltName = file.FileName;
                        tenderFile.TenderId = tender.Id;
                        tenderFile.DisplayOrder = i;

                 await _tenderFilesRepository.AddFileAsync(tenderFile);
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.TenderFiles), errorMessage!);
                        model = await _TendersRepository.InitializeTenderFormViewModelAsync(model);
                        return View("Form", model);
                    }
                }
            }


            return Json(new { success = true, id = tenderId });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var tender = await _TendersRepository.GetTenderWithGalleryImagesAsync(id);
            if (tender is null)
                return NotFound();

            // Row Level Permission
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null)
                return NotFound();

            var RowLevelPermission = await _rowPermission
                                          .HasRowLevelPermissionAsync(role.Id,
                                          TablesNames.Tenders,
                                          tender.Id,
                                          CrudOperations.Update);

            if (!RowLevelPermission && !(role.Name == AppRoles.SuperAdmin))
                return Redirect("/Identity/Account/AccessDenied");

            var model = new TenderFormViewModel()
            {
                Id = tender.Id,
                Title = tender.Title,
                Code = tender.Code,
                CopyPrice = tender.CopyPrice,
                StartDate = tender.StartDate,
                EndDate = tender.EndDate,
                EnvelopeOpeningDate = tender.EnvelopeOpeningDate,
                LastCopyPurchaseDate = tender.LastCopyPurchaseDate,
                Details = tender.Details,
                PricesOffered = tender.PricesOffered,
                MetaDescription = tender.MetaDescription,
                MetaKeywords = tender.MetaKeywords,
                TenderImageUrl = tender.TenderImageUrl,
                PricesOfferedAttachmentUrl = tender.PricesOfferedAttachmentUrl,
                InitialAwardFileUrl = tender.InitialAwardFileUrl,
                FinalAwardFileUrl = tender.FinalAwardFileUrl,
                Publish = tender.Publish,
                PublishPricesOffered = tender.PublishPricesOffered,
                SpecialOfferBlink = tender.SpecialOfferBlink,
                MoveToArchive = tender.MoveToArchive,
                BlinkStartDate = tender.BlinkStartDate,
                BlinkEndDate = tender.BlinkEndDate,
                SelectedMaterialIds = tender.Materials?.Select(c => c.MaterialId).ToList()
                ,
                ExistingFiles = tender.TenderFiles?.Select(f => new TenderFileViewModel
                {
                    FileUrl = f.FileUrl,
                    FileName = f.AltName,
                    DisplayOrder = f.DisplayOrder
                }).OrderBy(f => f.DisplayOrder)
                .ToList()
            };

            model = await _TendersRepository.InitializeTenderFormViewModelAsync(model);

            return View("Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, TenderFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _TendersRepository.InitializeTenderFormViewModelAsync(model);
                return View("Form", model);
            }

            var tender = await _TendersRepository.GetTenderByIdAsync(id);
            if (tender is null) return NotFound();

            // --- Row Level Permission ---
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null) return NotFound();

            var hasPermission = await _rowPermission.HasRowLevelPermissionAsync(
                role.Id,
                TablesNames.Tenders,
                tender.Id,
                CrudOperations.Update
            );

            if (!hasPermission && role.Name != AppRoles.SuperAdmin)
                return Redirect("/Identity/Account/AccessDenied");

            // --- Update tender properties ---
            tender.Title = model.Title;
            tender.Slug = _slugService.GenerateUniqueSlug(model.Title, nameof(Tender), tender.Id);
            tender.Code = model.Code;
            tender.CopyPrice = model.CopyPrice;
            tender.StartDate = model.StartDate;
            tender.EndDate = model.EndDate;
            tender.EnvelopeOpeningDate = model.EnvelopeOpeningDate;
            tender.LastCopyPurchaseDate = model.LastCopyPurchaseDate;
            tender.CreatedDate = DateTime.Now;
            tender.Details = model.Details;
            tender.PricesOffered = model.PricesOffered;
            tender.MetaDescription = model.MetaDescription;
            tender.MetaKeywords = model.MetaKeywords;
            tender.Publish = model.Publish;
            tender.PublishPricesOffered = model.PublishPricesOffered;
            tender.SpecialOfferBlink = model.SpecialOfferBlink;
            tender.MoveToArchive = model.MoveToArchive;
            tender.BlinkStartDate = model.BlinkStartDate;
            tender.BlinkEndDate = model.BlinkEndDate;

            // --- Add related materials ---
            if (model.SelectedMaterialIds != null && model.SelectedMaterialIds.Any())
                await _TendersRepository.AddRelatedMaterialsAsync(tender, model.SelectedMaterialIds);




            // ========== Initial Award File ==========
            if (model.InitialAwardFile is not null)
            {
                var fileName = $"{tender.Id}_InitialAward{Path.GetExtension(model.InitialAwardFile.FileName)}";

                var (isUploaded, errorMessage) = await _filesService.UploadASync(
                    model.InitialAwardFile,
                    fileName,
                    "/images/Tenders"
                );

                if (isUploaded)
                {
                    tender.InitialAwardFileUrl = fileName;
                }
                else
                {
                    ModelState.AddModelError(nameof(model.InitialAwardFile), errorMessage!);
                    model = await _TendersRepository.InitializeTenderFormViewModelAsync(model);
                    return View("Form", model);
                }
            }

            // ========== Final Award File ==========
            if (model.FinalAwardFile is not null)
            {
                var fileName = $"{tender.Id}_FinalAward{Path.GetExtension(model.FinalAwardFile.FileName)}";

                var (isUploaded, errorMessage) = await _filesService.UploadASync(
                    model.FinalAwardFile,
                    fileName,
                    "/images/Tenders"
                );

                if (isUploaded)
                {
                    tender.FinalAwardFileUrl = fileName;
                }
                else
                {
                    ModelState.AddModelError(nameof(model.FinalAwardFile), errorMessage!);
                    model = await _TendersRepository.InitializeTenderFormViewModelAsync(model);
                    return View("Form", model);
                }
            }

            // ========== Prices Offered Attachment ==========
            if (model.PricesOfferedAttachment is not null)
            {
                var fileName = $"{tender.Id}_PricesOffered{Path.GetExtension(model.PricesOfferedAttachment.FileName)}";

                var (isUploaded, errorMessage) = await _filesService.UploadASync(
                    model.PricesOfferedAttachment,
                    fileName,
                    "/images/Tenders"
                );

                if (isUploaded)
                {
                    tender.PricesOfferedAttachmentUrl = fileName;
                }
                else
                {
                    ModelState.AddModelError(nameof(model.PricesOfferedAttachment), errorMessage!);
                    model = await _TendersRepository.InitializeTenderFormViewModelAsync(model);
                    return View("Form", model);
                }
            }


            // --- Handle existing files ---
            var tenderOldFiles = await _tenderFilesRepository.GetFilesOfTenderAsync(tender.Id);
            var removedFileIds = model.RemovedFiles ?? new List<int>();

            // Delete removed files
            var filesToDelete = tenderOldFiles.Where(f => removedFileIds.Contains(f.Id)).ToList();
            foreach (var file in filesToDelete)
            {
                _tenderFilesRepository.DeleteFile(file); // أو DeleteRangeFiles إذا تدعم
                _filesService.Delete($"/documents/Tenders/{file.FileUrl}");
            }

            // Keep remaining old files
            var remainingOldFiles = tenderOldFiles.Where(f => !removedFileIds.Contains(f.Id)).ToList();

            // --- Add new uploaded files ---
            if (model.TenderFiles != null && model.TenderFiles.Any(f => f != null && f.Length > 0))
            {
                int startIndex = remainingOldFiles.Count; // Start after old files
                for (int i = 0; i < model.TenderFiles.Count; i++)
                {
                    var file = model.TenderFiles[i];
                    if (file == null || file.Length == 0) continue;

                    var tenderFile = new TenderFile();
                    var tenderFileName = $"{tender.Id}_{startIndex + i}{Path.GetExtension(file.FileName)}";

                    var (isUploaded, errorMessage) = await _filesService.UploadASync(file, tenderFileName, "/CMS/documents/Tenders");

                    if (!isUploaded)
                    {
                        ModelState.AddModelError(nameof(model.TenderFiles), errorMessage!);
                        model = await _TendersRepository.InitializeTenderFormViewModelAsync(model);
                        return View("Form", model);
                    }

                    tenderFile.FileUrl = tenderFileName;
                    tenderFile.AltName = file.FileName;
                    tenderFile.TenderId = tender.Id;
                    tenderFile.DisplayOrder = startIndex + i;

                    await _tenderFilesRepository.AddFileAsync(tenderFile);
                }
            }

            // --- Save tender ---
            _TendersRepository.Updatetender(tender);

            return Json(new { success = true, id = tender.Id });
        }








        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var tender = await _TendersRepository.GetTenderByIdAsync(id);

            if (tender is null)
                return NotFound();

            //Row level Permission Check
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null)
                return NotFound();
          


            var RowLevelPermission = await _rowPermission
                                        .HasRowLevelPermissionAsync(role.Id,
                                        TablesNames.Tenders,
                                        tender.Id,
                                        CrudOperations.Delete);

            if (!RowLevelPermission && !(role.Name == AppRoles.SuperAdmin))
                return StatusCode(403);





            _TendersRepository.Deletetender(tender);

            return StatusCode(200);
        }



        [HttpPost]
        [Authorize(Permissions.Tenders.Update)]
        public async Task<IActionResult> ToggleArchive(int id)
        {
            var tender = await _TendersRepository.GetTenderByIdAsync(id);
            if (tender == null) return NotFound();

            // Row-level permission check (similar to ToggleStatus)
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role == null) return Forbid();

            var hasPermission = await _rowPermission.HasRowLevelPermissionAsync(
                role.Id, TablesNames.Tenders, tender.Id, CrudOperations.Update
            );

            if (!hasPermission && role.Name != AppRoles.SuperAdmin)
                return Forbid();

            tender.MoveToArchive = !tender.MoveToArchive;
            _TendersRepository.Updatetender(tender);

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var tender = await _TendersRepository.GetTenderByIdAsync(id);

            if (tender is null)
                return StatusCode(402);

            //Row level Permission Check
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null)
                return NotFound();
         


            var RowLevelPermission = await _rowPermission
                                       .HasRowLevelPermissionAsync(role.Id,
                                       TablesNames.Tenders,
                                       tender.Id,
                                       CrudOperations.Update);

            if (!RowLevelPermission && !(role.Name == AppRoles.SuperAdmin))
                return StatusCode(403);

            tender.Publish = !tender.Publish;
            _TendersRepository.Updatetender(tender);

            return StatusCode(200);
        }


    }
}
