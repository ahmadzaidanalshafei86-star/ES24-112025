using AKM.Core.Entities;
using ES.Web.Areas.EsAdmin.Helpers;
using ES.Web.Areas.EsAdmin.Models;
using ES.Web.Areas.EsAdmin.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ES.Web.Controllers
{
    [Area("EsAdmin")]
    public class PurchaseOrdersTranslatesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PurchaseOrderTranslatesRepository _purchaseOrderTranslatesRepository;
        private readonly PurchaseOrdersRepository _purchaseOrdersRepository;
        private readonly RowPermission _rowPermission;

        public PurchaseOrdersTranslatesController(
            PurchaseOrdersRepository purchaseOrdersRepository,
            PurchaseOrderTranslatesRepository purchaseOrderTranslatesRepository,
            RoleManager<IdentityRole> roleManager,
            RowPermission rowPermission)
        {
            _purchaseOrdersRepository = purchaseOrdersRepository;
            _purchaseOrderTranslatesRepository = purchaseOrderTranslatesRepository;
            _roleManager = roleManager;
            _rowPermission = rowPermission;
        }

        // ------------------ Index ------------------
        [Authorize(Permissions.PurchaseOrders.Read)]
        public async Task<IActionResult> Index(int purchaseOrderId)
        {
            var purchaseOrder = await _purchaseOrdersRepository.GetPurchaseOrderByIdWithTranslationsAsync(purchaseOrderId);
            if (purchaseOrder == null) return NotFound();

            var model = new PurchaseOrderTranslatesViewModel
            {
                PurchaseOrderId = purchaseOrderId,
                PurchaseOrderTitle = purchaseOrder.Title,
                PurchaseOrderDefaultLang = purchaseOrder.Language?.Code,
                CreatedDate = purchaseOrder.CreatedDate,
                PreEnteredTranslations = purchaseOrder.PurchaseOrderTranslates?.ToList()
            };

            return View(model);
        }

        // ------------------ Create GET ------------------
        [HttpGet]
        [Authorize(Permissions.PurchaseOrders.Create)]
        public async Task<IActionResult> Create(int purchaseOrderId)
        {
            var purchaseOrder = await _purchaseOrdersRepository.GetPurchaseOrderByIdAsync(purchaseOrderId);
            if (purchaseOrder == null) return NotFound();

            var model = await _purchaseOrderTranslatesRepository.InitializePurchaseOrderTranslatesFormViewModelAsync(purchaseOrderId);

            // Pre-fill main PurchaseOrder info
            model.PurchaseOrderId = purchaseOrderId;
            model.Title = purchaseOrder.Title;
            model.Details = purchaseOrder.Details;
            model.MetaDescription = purchaseOrder.MetaDescription;
            model.MetaKeywords = purchaseOrder.MetaKeywords;

            return View("Form", model);
        }

        // ------------------ Create POST ------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Permissions.PurchaseOrders.Create)]
        public async Task<IActionResult> Create(PurchaseOrderTranslationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _purchaseOrderTranslatesRepository.InitializePurchaseOrderTranslatesFormViewModelAsync(model.PurchaseOrderId);
                return View("Form", model);
            }

            var purchaseOrderTranslate = new PurchaseOrderTranslate
            {
                Title = model.Title,
                Details = model.Details,
                MetaDescription = model.MetaDescription,
                MetaKeywords = model.MetaKeywords,
                CreatedDate = DateTime.UtcNow,
                LanguageId = (int)model.LanguageId,
                PurchaseOrderId = model.PurchaseOrderId
            };

            await _purchaseOrderTranslatesRepository.AddPurchaseOrderTranslateAsync(purchaseOrderTranslate);

            return RedirectToAction("Index", new { purchaseOrderId = model.PurchaseOrderId });
        }

        // ------------------ Edit GET ------------------
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int purchaseOrderId, int translateId)
        {
            var translate = await _purchaseOrderTranslatesRepository.GetPurchaseOrderTranslateByIdAsync(translateId);
            if (translate == null) return NotFound();

            // Permission check
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null) return NotFound();

            var rowLevelPermission = await _rowPermission.HasRowLevelPermissionAsync(
                role.Id,
                TablesNames.PurchaseOrders,
                purchaseOrderId,
                CrudOperations.Update
            );

            if (!rowLevelPermission && role.Name != AppRoles.SuperAdmin)
                return Redirect("/Identity/Account/AccessDenied");

            var model = new PurchaseOrderTranslationFormViewModel
            {
                TranslationId = translateId,
                Title = translate.Title,
                Details = translate.Details,
                MetaDescription = translate.MetaDescription,
                MetaKeywords = translate.MetaKeywords
            };

            model = await _purchaseOrderTranslatesRepository.InitializePurchaseOrderTranslatesFormViewModelAsync(purchaseOrderId, model);
            model.PurchaseOrderId = purchaseOrderId;

            return View("Form", model);
        }

        // ------------------ Edit POST ------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(PurchaseOrderTranslationFormViewModel model, int purchaseOrderId)
        {
            if (!ModelState.IsValid)
            {
                model = await _purchaseOrderTranslatesRepository.InitializePurchaseOrderTranslatesFormViewModelAsync(purchaseOrderId, model);
                model.PurchaseOrderId = purchaseOrderId;
                return View("Form", model);
            }

            var translate = await _purchaseOrderTranslatesRepository.GetPurchaseOrderTranslateByIdAsync(model.TranslationId);
            if (translate == null) return NotFound();

            // Permission check
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null) return NotFound();

            var rowLevelPermission = await _rowPermission.HasRowLevelPermissionAsync(
                role.Id,
                TablesNames.PurchaseOrders,
                purchaseOrderId,
                CrudOperations.Update
            );

            if (!rowLevelPermission && role.Name != AppRoles.SuperAdmin)
                return Redirect("/Identity/Account/AccessDenied");

            translate.Title = model.Title;
            translate.Details = model.Details;
            translate.MetaDescription = model.MetaDescription;
            translate.MetaKeywords = model.MetaKeywords;

            _purchaseOrderTranslatesRepository.UpdatePurchaseOrderTranslate(translate);

            return RedirectToAction("Index", new { purchaseOrderId = model.PurchaseOrderId });
        }

        // ------------------ Delete ------------------
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var translate = await _purchaseOrderTranslatesRepository.GetPurchaseOrderTranslateByIdAsync(id);
            if (translate == null) return StatusCode(404);

            // Permission check
            var role = await _roleManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Role));
            if (role is null) return NotFound();

            var rowLevelPermission = await _rowPermission.HasRowLevelPermissionAsync(
                role.Id,
                TablesNames.PurchaseOrders,
                id,
                CrudOperations.Delete
            );

            if (!rowLevelPermission && role.Name != AppRoles.SuperAdmin)
                return StatusCode(403);

            var result = await _purchaseOrderTranslatesRepository.DeleteTranslationAsync(id);
            return result ? StatusCode(200) : BadRequest();
        }
    }
}
