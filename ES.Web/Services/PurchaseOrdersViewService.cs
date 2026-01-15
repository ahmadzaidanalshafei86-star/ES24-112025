using ES.Core.Entities;
using ES.Web.Helpers;
using ES.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ES.Web.Services
{
    public class PurchaseOrdersViewService
    {
        private readonly ApplicationDbContext _context;

        private readonly string filesPath = "CMS/documents/PurchaseOrders/";

        public PurchaseOrdersViewService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== Helper: Build full path =====
        private string? BuildFilePath(string? fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                return null;

            return filesPath + fileUrl;
        }

        // ========== Get All Purchase Orders ==========
        public async Task<PurchaseOrdersViewViewModel> GetAllAsync()
        {
            var model = new PurchaseOrdersViewViewModel();
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

            // Get data + translations
            var orders = await _context.PurchaseOrders
                .AsNoTracking()
                .Include(o => o.PurchaseOrderTranslates)
                .Where(o => o.Publish && !o.MoveToArchive)
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync();

            // Apply translations + file paths
            var list = orders.Select(o =>
            {
                var trans = o.PurchaseOrderTranslates?
                    .FirstOrDefault(x => x.LanguageId == languageId);

               

                return new PurchaseOrder
                {
                    Id = o.Id,
                    Code = o.Code,

                    Title = trans?.Title ?? o.Title,
                    Slug = o.Slug,

                    StartDate = o.StartDate,
                    EndDate = o.EndDate,

                    Details = trans?.Details ?? o.Details,
                    MetaDescription = trans?.MetaDescription ?? o.MetaDescription,
                    MetaKeywords = trans?.MetaKeywords ?? o.MetaKeywords,

                    PurchaseOrderImageUrl = o.PurchaseOrderImageUrl,
                    PurchaseOrderImageAltName = o.PurchaseOrderImageAltName,

                 

                    Publish = o.Publish,
                    MoveToArchive = o.MoveToArchive,

                    CreatedDate = o.CreatedDate,
                    UpdatedDate = o.UpdatedDate
                };
            }).ToList();

            model.PurchaseOrders = list;
            return model;
        }

        // ========== Get by ID ==========
        public async Task<PurchaseOrder?> GetByIdAsync(int id)
        {
            var order = await _context.PurchaseOrders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order != null)
            {
               
            }

            return order;
        }
    }
}
