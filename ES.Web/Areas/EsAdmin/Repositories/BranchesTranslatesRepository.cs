



using ES.Core.Entities;
using ES.Web.Areas.EsAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ES.Web.Areas.EsAdmin.Repositories
{
    public class BranchesTranslatesRepository
    {
        private readonly ApplicationDbContext _context;
        public BranchesTranslatesRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<BranchTranslate> GetBranchTranslateByIdAsync(int id)
        {
            var translate = await _context.BranchesTranslate.FindAsync(id);

            if (translate == null)
                throw new Exception(message: "Branch not found");

            return translate;

        }

        public async Task<IEnumerable<SelectListItem>> GetLanguagesAsync(int branchId)
        {

            // Get all language IDs that already have translations for the given Branch
            var translatedLanguageIds = await _context.BranchesTranslate
                .Where(ct => ct.BranchId == branchId)
                .Select(ct => ct.LanguageId)
                .ToListAsync();

            // to know the default lang of the branch and not showing it in the dropdown
            var branch = await _context.Branches
                .Include(c => c.Language)
                .SingleOrDefaultAsync(c => c.Id == branchId);

            if (branch == null)
                throw new Exception(message: "Branch not found");

            return await _context.Languages
           .Where(l => l.Code != branch.Language.Code && !translatedLanguageIds.Contains(l.Id))
          .Select(th => new SelectListItem
          {
              Value = th.Id.ToString(),
              Text = th.Name
          })
          .ToListAsync();
        }

        public async Task<BranchTranslationFormViewModel> InitializeBranchTranslatesFormViewModelAsync(int branchId, BranchTranslationFormViewModel? model = null)
        {
            model ??= new BranchTranslationFormViewModel(); // Initialize a new model if none is provided.

            model.Languages = await GetLanguagesAsync(branchId);
            return model;
        }

        public async Task<int> AddBranchTranslateAsync(BranchTranslate branchTranslate)
        {
            await _context.BranchesTranslate.AddAsync(branchTranslate);
            await _context.SaveChangesAsync();
            return branchTranslate.Id;
        }


        public void UpdateBranch(BranchTranslate branchTranslate)
        {
            _context.BranchesTranslate.Update(branchTranslate);
            _context.SaveChanges();
        }

        public async Task<bool> DeleteTranslationAsync(int id)
        {
            var translate = await GetBranchTranslateByIdAsync(id);

            _context.BranchesTranslate.Remove(translate);
            await _context.SaveChangesAsync();

            return true;

        }
    }

}

