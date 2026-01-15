using AKM.Core.Entities;
using ES.Web.Areas.EsAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ES.Web.Areas.EsAdmin.Repositories
{
    public class PurchaseOrderTranslatesRepository
    {
        private readonly ApplicationDbContext _context;
        public PurchaseOrderTranslatesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PurchaseOrderTranslationFormViewModel> InitializePurchaseOrderTranslatesFormViewModelAsync(int purchaseOrderId, PurchaseOrderTranslationFormViewModel? model = null)
        {
            model ??= new PurchaseOrderTranslationFormViewModel();
            model.Languages = await GetLanguagesAsync(purchaseOrderId);
            return model;
        }

        public async Task<PurchaseOrderTranslate> GetPurchaseOrderTranslateByIdAsync(int id)
        {
            var translate = await _context.PurchaseOrderTranslates
                .Where(t => t.Id == id)
                .Include(t => t.PurchaseOrder)
                .FirstOrDefaultAsync();

            if (translate == null)
                throw new Exception("PurchaseOrder translation not found");

            return translate;
        }

        public async Task<int> AddPurchaseOrderTranslateAsync(PurchaseOrderTranslate purchaseOrderTranslate)
        {
            await _context.PurchaseOrderTranslates.AddAsync(purchaseOrderTranslate);
            await _context.SaveChangesAsync();
            return purchaseOrderTranslate.Id;
        }

        public async Task<bool> DeleteTranslationAsync(int id)
        {
            var translate = await GetPurchaseOrderTranslateByIdAsync(id);
            _context.PurchaseOrderTranslates.Remove(translate);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<IEnumerable<SelectListItem>> GetLanguagesAsync(int purchaseOrderId)
        {
            // Get language IDs that already have translations for the purchase order
            var translatedLanguageIds = await _context.PurchaseOrderTranslates
                .Where(t => t.PurchaseOrderId == purchaseOrderId)
                .Select(t => t.LanguageId)
                .ToListAsync();

            // Get purchase order + default language
            var purchaseOrder = await _context.PurchaseOrders
                .Include(t => t.Language)
                .SingleOrDefaultAsync(t => t.Id == purchaseOrderId);

            if (purchaseOrder == null)
                throw new Exception("PurchaseOrder not found");

            // Exclude default language + already translated languages
            return await _context.Languages
                .Where(l => l.Code != purchaseOrder.Language.Code && !translatedLanguageIds.Contains(l.Id))
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                })
                .ToListAsync();
        }

        public void UpdatePurchaseOrderTranslate(PurchaseOrderTranslate purchaseOrderTranslate)
        {
            _context.PurchaseOrderTranslates.Update(purchaseOrderTranslate);
            _context.SaveChanges();
        }
    }
}
