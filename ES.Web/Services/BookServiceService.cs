using ES.Web.Helpers;
using ES.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ES.Web.Services
{
    public class BookServiceService
    {
        private readonly ApplicationDbContext _context;

        public BookServiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a branch with its translated Hangars and Refrigators by branchId.
        /// </summary>
        public async Task<Branch?> GetBranchWithDetailsAsync(int branchId)
        {
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

            var branch = await _context.Branches
                .Include(b => b.RelatedHangars)
                    .ThenInclude(h => h.HangarsTranslates)
                .Include(b => b.RelatedRefrigators)
                    .ThenInclude(r => r.RefrigatorsTranslates)
                .FirstOrDefaultAsync(b => b.Id == branchId);

            if (branch == null)
                return null;

            // Translate Hangars
            branch.RelatedHangars = branch.RelatedHangars?
                .Select(h => new Hangar
                {
                    Id = h.Id,
                    BranchId = h.BranchId,
                    Name = h.HangarsTranslates
                        .FirstOrDefault(t => t.LanguageId == languageId)?.Name ?? h.Name,
                    Size = h.HangarsTranslates
                        .FirstOrDefault(t => t.LanguageId == languageId)?.Size ?? h.Size,
                    Type = h.HangarsTranslates
                        .FirstOrDefault(t => t.LanguageId == languageId)?.Type ?? h.Type
                })
                .ToList();

            // Translate Refrigators
            branch.RelatedRefrigators = branch.RelatedRefrigators?
                .Select(r => new Refrigator
                {
                    Id = r.Id,
                    BranchId = r.BranchId,
                    Name = r.RefrigatorsTranslates
                        .FirstOrDefault(t => t.LanguageId == languageId)?.Name ?? r.Name,
                    Size = r.RefrigatorsTranslates
                        .FirstOrDefault(t => t.LanguageId == languageId)?.Size ?? r.Size,
                    Type = r.RefrigatorsTranslates
                        .FirstOrDefault(t => t.LanguageId == languageId)?.Type ?? r.Type
                })
                .ToList();

            return branch;
        }

        /// <summary>
        /// Initializes the BookService form view model with translated branches.
        /// </summary>
        public async Task<BookServiceFormViewModel> InitializeBookServiceFormViewModel()
        {
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

            // جلب الفروع مع الترجمات
            var branchesData = await _context.Branches
                .Include(b => b.BranchesTranslates)
                .ToListAsync();

            // بعد جلب البيانات، التعامل مع null
            var branches = branchesData.Select(b => new BranchSelectViewModel
            {
                Id = b.Id,
                Name = b.BranchesTranslates.FirstOrDefault(t => t.LanguageId == languageId)?.Name ?? b.Name
            }).ToList();

            return new BookServiceFormViewModel
            {
                Branches = branches
            };
        }

        /// <summary>
        /// Saves a BookServiceRequest with its Hangars and Refrigators.
        /// </summary>
        public async Task SaveBookServiceRequestAsync(BookServiceRequest request)
        {
            // ربط Hangars بالطلب
            if (request.Hangars != null)
            {
                foreach (var hangar in request.Hangars)
                {
                    hangar.BookServiceRequest = request;
                }
            }

            // ربط Refrigators بالطلب
            if (request.Refrigators != null)
            {
                foreach (var refrigator in request.Refrigators)
                {
                    refrigator.BookServiceRequest = request;
                }
            }

            _context.BookServiceRequests.Add(request);
            await _context.SaveChangesAsync();
        }
    }
}
