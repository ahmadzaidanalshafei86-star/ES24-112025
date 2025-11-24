namespace ES.Web.Areas.EsAdmin.Repositories
{
    public class TenderFilesRepository
    {
        private readonly ApplicationDbContext _context;
        public TenderFilesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // إضافة ملف جديد
        public async Task<int> AddFileAsync(TenderFile file)
        {
            await _context.TendersFiles.AddAsync(file);
            await _context.SaveChangesAsync();
            return file.Id;
        }

        // حذف مجموعة ملفات
        public void DeleteRangeFiles(IList<TenderFile> files)
        {
            if (files == null || !files.Any()) return;
            _context.TendersFiles.RemoveRange(files);
            _context.SaveChanges();
        }

        // حذف ملف واحد
        public void DeleteFile(TenderFile file)
        {
            if (file == null) return;
            _context.TendersFiles.Remove(file);
            _context.SaveChanges();
        }

        // جلب كل الملفات الخاصة بمناقصة معينة
        public async Task<IList<TenderFile>> GetFilesOfTenderAsync(int tenderId)
        {
            return await _context.TendersFiles
                .Where(pf => pf.TenderId == tenderId)
                .OrderBy(pf => pf.DisplayOrder)
                .ToListAsync();
        }

        // جلب ملف واحد بالـ Id
        public async Task<TenderFile?> GetFileByIdAsync(int id)
        {
            return await _context.TendersFiles.FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
