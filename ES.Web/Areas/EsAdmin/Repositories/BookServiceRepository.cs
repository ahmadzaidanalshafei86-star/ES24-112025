using ES.Core.Entities;
using ES.Web.Areas.EsAdmin.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ES.Web.Areas.EsAdmin.Repositories
{
    public class BookServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public BookServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// جلب كل طلبات BookService مع معلومات الفرع وHangars وRefrigators
        /// </summary>
        public async Task<List<BookServiceViewModel>> GetBookServiceInfoAsync()
        {
            var requests = await _context.BookServiceRequests
                .Include(r => r.Hangars)
                .Include(r => r.Refrigators)
                .Include(r => r.Branch) // يجب أن يكون لديك navigation property Branch في BookServiceRequest
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new BookServiceViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    PhoneNumber = r.PhoneNumber,
                    EmailAddress = r.EmailAddress,
                    Notes = r.Notes,
                    BranchName = r.Branch != null ? r.Branch.Name : string.Empty,
                    CreatedAt = r.CreatedAt,
                    Hangars = r.Hangars.Select(h => h.Name).ToList(),
                    Refrigators = r.Refrigators.Select(rf => rf.Name).ToList()
                })
                .ToListAsync();

            return requests;
        }

        /// <summary>
        /// جلب بيانات طلب واحد بالتفصيل حسب Id
        /// </summary>
        public async Task<BookServiceViewModel?> GetBookServiceRequestByIdAsync(int id)
        {
            var request = await _context.BookServiceRequests
                .Include(r => r.Hangars)
                .Include(r => r.Refrigators)
                .Include(r => r.Branch)
                .Where(r => r.Id == id)
                .Select(r => new BookServiceViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    PhoneNumber = r.PhoneNumber,
                    EmailAddress = r.EmailAddress,
                    Notes = r.Notes,
                    BranchName = r.Branch != null ? r.Branch.Name : string.Empty,
                    CreatedAt = r.CreatedAt,
                    Hangars = r.Hangars.Select(h => h.Name).ToList(),
                    Refrigators = r.Refrigators.Select(rf => rf.Name).ToList()
                })
                .FirstOrDefaultAsync();

            return request;
        }
    }
}
