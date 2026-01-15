using ES.Core.Entities;
using ES.Web.Areas.EsAdmin.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ES.Web.Areas.EsAdmin.Repositories
{
    public class InquiryRepository
    {
        private readonly ApplicationDbContext _context;

        public InquiryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// جلب كل طلبات Inquiry مع اسم العطاء ورابط المرفق بدون علاقة مباشرة في الـ Entity
        /// </summary>
        public async Task<List<InquiryViewModel>> GetInquiryInfoAsync()
        {
            var requests = await (from inquiry in _context.TenderInquiries
                                  join tender in _context.Tenders
                                      on inquiry.TenderId equals tender.Id
                                  orderby inquiry.CreatedAt descending
                                  select new InquiryViewModel
                                  {
                                      Id = inquiry.Id,
                                      Name = inquiry.Name,
                                      Phone = inquiry.Phone,
                                      Email = inquiry.Email,
                                      InquiryText = inquiry.InquiryText,
                                      TenderId = inquiry.TenderId,
                                      TenderName = tender.Title,
                                      AttachmentUrl = inquiry.AttachmentUrl, // رابط المرفق
                                      CreatedAt = inquiry.CreatedAt
                                  }).ToListAsync();

            return requests;
        }

        /// <summary>
        /// جلب بيانات طلب واحد بالتفصيل حسب Id مع اسم العطاء ورابط المرفق
        /// </summary>
        public async Task<InquiryViewModel?> GetInquiryRequestByIdAsync(int id)
        {
            var request = await (from inquiry in _context.TenderInquiries
                                 join tender in _context.Tenders
                                     on inquiry.TenderId equals tender.Id
                                 where inquiry.Id == id
                                 select new InquiryViewModel
                                 {
                                     Id = inquiry.Id,
                                     Name = inquiry.Name,
                                     Phone = inquiry.Phone,
                                     Email = inquiry.Email,
                                     InquiryText = inquiry.InquiryText,
                                     TenderId = inquiry.TenderId,
                                     TenderName = tender.Title,
                                     AttachmentUrl = inquiry.AttachmentUrl, // رابط المرفق
                                     CreatedAt = inquiry.CreatedAt
                                 }).FirstOrDefaultAsync();

            return request;
        }
    }
}
