using ES.Web.Helpers;
using ES.Web.Models;

namespace ES.Web.Services
{
    public class SuppliersService
    {
        private readonly ApplicationDbContext _context;
        public SuppliersService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<SupplierFormViewModel> InitializeSupplierFormViewModel()
        {
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

            var materials = await _context.Materials
                .Include(m => m.MaterialsTranslates)
                .Select(m => new MaterialCheckboxViewModel
                {
                    Id = m.Id,
                    Name = m.MaterialsTranslates
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => t.Name)
                        .FirstOrDefault() ?? m.Name, 
                    IsSelected = false
                })
                .ToListAsync();

            return new SupplierFormViewModel
            {
                Materials = materials
            };
        }

    }
}
