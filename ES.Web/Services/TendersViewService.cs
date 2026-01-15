using ES.Core.Entities; // مهم
using ES.Web.Helpers;
using ES.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ES.Web.Services
{
    public class TendersViewService
    {
        private readonly ApplicationDbContext _context;

        private readonly string tenderFilesPath = "CMS/documents/Tenders/";
        

        public TendersViewService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== Helper: Build full path =====
        private string? BuildFilePath(string? fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return null;

           

            // بشكل افتراضي نضعه ضمن ملفات العطاء
            return tenderFilesPath + fileUrl;
        }

        // ========== Get All Tenders ==========
        //public async Task<TendersViewViewModel> GetAllAsync()
        //{
        //    var model = new TendersViewViewModel();
        //    var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);
        //    // 1️⃣ جلب البيانات من قاعدة البيانات أولاً
        //    var tenders = await _context.Tenders
        //        .AsNoTracking()
        //        .Where(s => s.Publish && !s.MoveToArchive)
        //        .OrderByDescending(s => s.CreatedDate)
        //        .ToListAsync();

        //    // 2️⃣ تعديل مسارات الملفات على الذاكرة
        //    foreach (var t in tenders)
        //    {
        //        t.InitialAwardFileUrl = BuildFilePath(t.InitialAwardFileUrl);
        //        t.FinalAwardFileUrl = BuildFilePath(t.FinalAwardFileUrl);
        //        t.PricesOfferedAttachmentUrl = BuildFilePath(t.PricesOfferedAttachmentUrl);
        //    }

        //    model.Tenders = tenders;
        //    return model;
        //}
        // ========== Get All Tenders ==========
        public async Task<TendersViewViewModel> GetAllAsync()
        {
            var model = new TendersViewViewModel();
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

            // 1️⃣ Get tenders + translations
            var tenders = await _context.Tenders
                .AsNoTracking()
                .Include(t => t.TenderTranslates)
                .Where(s => s.Publish && !s.MoveToArchive)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();

            // 2️⃣ Process data + apply translation
            var list = tenders.Select(t =>
            {
                // Bring correct translation
                var trans = t.TenderTranslates?
                    .FirstOrDefault(x => x.LanguageId == languageId);

                // Fix file paths
                t.InitialAwardFileUrl = BuildFilePath(t.InitialAwardFileUrl);
                t.FinalAwardFileUrl = BuildFilePath(t.FinalAwardFileUrl);
                t.PricesOfferedAttachmentUrl = BuildFilePath(t.PricesOfferedAttachmentUrl);

                // Return translated tender object (new DTO if needed)
                return new Tender
                {
                    Id = t.Id,
                    Code = t.Code,
                    CopyPrice = t.CopyPrice,

                    // Title translation
                    Title = trans?.Title ?? t.Title,

                    Slug = t.Slug,

                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    EnvelopeOpeningDate = t.EnvelopeOpeningDate,
                    LastCopyPurchaseDate = t.LastCopyPurchaseDate,

                    Details = trans?.Details ?? t.Details,
                    PricesOffered = trans?.PricesOffered ?? t.PricesOffered,
                    MetaDescription = trans?.MetaDescription ?? t.MetaDescription,
                    MetaKeywords = trans?.MetaKeywords ?? t.MetaKeywords,

                    TenderImageUrl = t.TenderImageUrl,
                    TenderImageAltName = t.TenderImageAltName,

                    PricesOfferedAttachmentUrl = t.PricesOfferedAttachmentUrl,
                    InitialAwardFileUrl = t.InitialAwardFileUrl,
                    FinalAwardFileUrl = t.FinalAwardFileUrl,

                    Publish = t.Publish,
                    PublishPricesOffered = t.PublishPricesOffered,
                    SpecialOfferBlink = t.SpecialOfferBlink,
                    MoveToArchive = t.MoveToArchive,

                    BlinkStartDate = t.BlinkStartDate,
                    BlinkEndDate = t.BlinkEndDate,

                    CreatedDate = t.CreatedDate,
                    UpdatedDate = t.UpdatedDate
                };
            }).ToList();

            model.Tenders = list;
            return model;
        }

        // ========== Get By ID ==========
        public async Task<Tender?> GetByIdAsync(int id)
        {
            var tender = await _context.Tenders
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (tender != null)
            {
                tender.InitialAwardFileUrl = BuildFilePath(tender.InitialAwardFileUrl);
                tender.FinalAwardFileUrl = BuildFilePath(tender.FinalAwardFileUrl);
                tender.PricesOfferedAttachmentUrl = BuildFilePath(tender.PricesOfferedAttachmentUrl);
            }

            return tender;
        }
    }
}
