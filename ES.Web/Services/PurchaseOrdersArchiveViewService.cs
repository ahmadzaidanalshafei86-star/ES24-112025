using ES.Core.Entities; // مهم
using ES.Web.Helpers;
using ES.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ES.Web.Services
{
    public class PurchaseOrdersArchiveViewService
    {
        private readonly ApplicationDbContext _context;

        private readonly string purchaseOrderFilesPath = "CMS/documents/PurchaseOrders/";

        public PurchaseOrdersArchiveViewService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== Helper: Build full path =====
        private string? BuildFilePath(string? fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return null;

            // بشكل افتراضي نضعه ضمن ملفات PurchaseOrders
            return purchaseOrderFilesPath + fileUrl;
        }

        // ========== Get All PurchaseOrdersArchive ==========
        public async Task<PurchaseOrdersArchiveViewViewModel> GetAllAsync()
        {
            var model = new PurchaseOrdersArchiveViewViewModel();
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

            // 1️⃣ Get purchase orders + translations
            var purchaseOrders = await _context.PurchaseOrders
                .AsNoTracking()
                .Include(po => po.PurchaseOrderTranslates)
                .Where(s => s.Publish && s.MoveToArchive)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();

            // 2️⃣ Process data + apply translation
            var list = purchaseOrders.Select(po =>
            {
                var trans = po.PurchaseOrderTranslates?
                    .FirstOrDefault(x => x.LanguageId == languageId);


                // Return translated PurchaseOrder object (DTO if needed)
                return new PurchaseOrder
                {
                    Id = po.Id,
                    Code = po.Code,

                    Title = trans?.Title ?? po.Title,
                    Slug = po.Slug,

                    StartDate = po.StartDate,
                    EndDate = po.EndDate,
                    EnvelopeOpeningDate = po.EnvelopeOpeningDate,

                    Details = trans?.Details ?? po.Details,
                    MetaDescription = trans?.MetaDescription ?? po.MetaDescription,
                    MetaKeywords = trans?.MetaKeywords ?? po.MetaKeywords,

                    PurchaseOrderImageUrl = po.PurchaseOrderImageUrl,
                    PurchaseOrderImageAltName = po.PurchaseOrderImageAltName,

                    

                    Publish = po.Publish,
                   
                    MoveToArchive = po.MoveToArchive,

                 

                    CreatedDate = po.CreatedDate,
                    UpdatedDate = po.UpdatedDate
                };
            }).ToList();

            model.PurchaseOrdersArchive = list;
            return model;
        }

        // ========== Get By ID ==========
        public async Task<PurchaseOrder?> GetByIdAsync(int id)
        {
            var po = await _context.PurchaseOrders
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (po != null)
            {
               
            }

            return po;
        }
    }
}
