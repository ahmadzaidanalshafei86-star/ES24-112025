using ES.Core.Entities;
using ES.Web.Helpers;
using ES.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ES.Web.Services
{
    public class HangarsViewService
    {
        private readonly ApplicationDbContext _context;

        public HangarsViewService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================== Get Hangars By Branch ==================
        public async Task<HangarsViewViewModel> GetAllAsync(int branchId)
        {
            var model = new HangarsViewViewModel();

            // 1️⃣ Get current language
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

            // 2️⃣ Load original hangars + translations
            var hangars = await _context.Hangars
                .AsNoTracking()
                .Include(t => t.HangarsTranslates)
                .Where(h => h.BranchId == branchId)
                .OrderBy(h => h.Id)
                .ToListAsync();

            // 3️⃣ Apply translation (same style as Tenders)
            var list = hangars.Select(h =>
            {
                // Correct translation
                var trans = h.HangarsTranslates?
                    .FirstOrDefault(x => x.LanguageId == languageId);

                // Return translated Hangar (new object)
                return new Hangar
                {
                    Id = h.Id,
                    BranchId = h.BranchId,

                    // Translated fields
                    Name = trans?.Name ?? h.Name,
                    Size = trans?.Size ?? h.Size,
                    Type = trans?.Type ?? h.Type,

                    // Keep original relations if needed
                    HangarsTranslates = h.HangarsTranslates,
                    Branch = h.Branch
                };
            }).ToList();

            // 4️⃣ Assign to model
            model.Hangars = list;
            return model;
        }

    }
}
