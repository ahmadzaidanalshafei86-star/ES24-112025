using ES.Core.Entities;
using ES.Web.Helpers;
using ES.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ES.Web.Services
{
    public class RefrigeratorsViewService
    {
        private readonly ApplicationDbContext _context;

        public RefrigeratorsViewService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================== Get Refrigerators By Branch ==================
        public async Task<RefrigeratorsViewViewModel> GetAllAsync(int branchId)
        {
            var model = new RefrigeratorsViewViewModel();

            // 1️⃣ Get current language
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

            // 2️⃣ Load original Refrigerators + translations
            var Refrigerators = await _context.Refrigators
                .AsNoTracking()
                .Include(t => t.RefrigatorsTranslates)
                .Where(h => h.BranchId == branchId)
                .OrderBy(h => h.Id)
                .ToListAsync();

            // 3️⃣ Apply translation (same style as Tenders)
            var list = Refrigerators.Select(h =>
            {
                // Correct translation
                var trans = h.RefrigatorsTranslates?
                    .FirstOrDefault(x => x.LanguageId == languageId);

                // Return translated Refrigerator (new object)
                return new Refrigator
                {
                    Id = h.Id,
                    BranchId = h.BranchId,

                    // Translated fields
                    Name = trans?.Name ?? h.Name,
                    Size = trans?.Size ?? h.Size,
                    Type = trans?.Type ?? h.Type,

                    // Keep original relations if needed
                    RefrigatorsTranslates = h.RefrigatorsTranslates,
                    Branch = h.Branch
                };
            }).ToList();

            // 4️⃣ Assign to model
            model.Refrigerators = list;
            return model;
        }

    }
}
