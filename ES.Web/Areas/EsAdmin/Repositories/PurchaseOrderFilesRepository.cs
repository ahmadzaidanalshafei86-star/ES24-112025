namespace ES.Web.Areas.EsAdmin.Repositories
{
    public class PurchaseOrderFilesRepository
    {
        private readonly ApplicationDbContext _context;
        public PurchaseOrderFilesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // إضافة ملف جديد
        public async Task<int> AddFileAsync(PurchaseOrderFile file)
        {
            await _context.PurchaseOrdersFiles.AddAsync(file);
            await _context.SaveChangesAsync();
            return file.Id;
        }

        // حذف مجموعة ملفات
        public void DeleteRangeFiles(IList<PurchaseOrderFile> files)
        {
            if (files == null || !files.Any()) return;
            _context.PurchaseOrdersFiles.RemoveRange(files);
            _context.SaveChanges();
        }

        // حذف ملف واحد
        public void DeleteFile(PurchaseOrderFile file)
        {
            if (file == null) return;
            _context.PurchaseOrdersFiles.Remove(file);
            _context.SaveChanges();
        }

        // جلب كل الملفات الخاصة بأمر شراء معين
        public async Task<IList<PurchaseOrderFile>> GetFilesOfPurchaseOrderAsync(int purchaseOrderId)
        {
            return await _context.PurchaseOrdersFiles
                .Where(pf => pf.PurchaseOrderId == purchaseOrderId)
                .OrderBy(pf => pf.DisplayOrder)
                .ToListAsync();
        }

        // جلب ملف واحد بالـ Id
        public async Task<PurchaseOrderFile?> GetFileByIdAsync(int id)
        {
            return await _context.PurchaseOrdersFiles.FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
