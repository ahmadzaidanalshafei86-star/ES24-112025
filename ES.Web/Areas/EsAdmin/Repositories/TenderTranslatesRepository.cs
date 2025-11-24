using AKM.Core.Entities;
using ES.Web.Areas.EsAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ES.Web.Areas.EsAdmin.Repositories
{
    public class TenderTranslatesRepository
    {
        private readonly ApplicationDbContext _context;
        public TenderTranslatesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TenderTranslationFormViewModel> InitializeTenderTranslatesFormViewModelAsync(int tenderId, TenderTranslationFormViewModel? model = null)
        {
            model ??= new TenderTranslationFormViewModel();
            model.Languages = await GetLanguagesAsync(tenderId);
            return model;
        }

        public async Task<TenderTranslate> GetTenderTranslateByIdAsync(int id)
        {
            var translate = await _context.TenderTranslates.Where(t => t.Id == id)
                .Include(t => t.Tender)
                .FirstOrDefaultAsync();

            if (translate == null)
                throw new Exception("Tender translation not found");

            return translate;
        }

        public async Task<int> AddTenderTranslateAsync(TenderTranslate tenderTranslate)
        {
            await _context.TenderTranslates.AddAsync(tenderTranslate);
            await _context.SaveChangesAsync();
            return tenderTranslate.Id;
        }

        public async Task<bool> DeleteTranslationAsync(int id)
        {
            var translate = await GetTenderTranslateByIdAsync(id);
            _context.TenderTranslates.Remove(translate);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<IEnumerable<SelectListItem>> GetLanguagesAsync(int tenderId)
        {
            // Get language IDs that already have translations for the tender
            var translatedLanguageIds = await _context.TenderTranslates
                .Where(t => t.TenderId == tenderId)
                .Select(t => t.LanguageId)
                .ToListAsync();

            // Get tender + default language
            var tender = await _context.Tenders
                .Include(t => t.Language)
                .SingleOrDefaultAsync(t => t.Id == tenderId);

            if (tender == null)
                throw new Exception("Tender not found");

            // Exclude default language + already translated languages
            return await _context.Languages
                .Where(l => l.Code != tender.Language.Code && !translatedLanguageIds.Contains(l.Id))
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                })
                .ToListAsync();
        }

        public void UpdateTenderTranslate(TenderTranslate tenderTranslate)
        {
            _context.TenderTranslates.Update(tenderTranslate);
            _context.SaveChanges();
        }
    }
}
