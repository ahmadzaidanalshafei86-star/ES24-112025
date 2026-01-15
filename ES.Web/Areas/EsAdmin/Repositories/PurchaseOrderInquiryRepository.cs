


using ES.Core.Entities;
using ES.Web.Areas.EsAdmin.Models;
using ES.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ES.Web.Areas.EsAdmin.Repositories
{
    public class PurchaseOrderInquiryRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderInquiryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// جلب كل طلبات Inquiry مع اسم العطاء ورابط المرفق بدون علاقة مباشرة في الـ Entity
        /// </summary>
        public async Task<List<PurchaseOrderAdminInquiryViewModel>> GetInquiryInfoAsync()
        {
            var requests = await (from inquiry in _context.PurchaseOrderInquiries
                                  join purchaseOrder in _context.PurchaseOrders
                                      on inquiry.PurchaseOrderId equals purchaseOrder.Id
                                  orderby inquiry.CreatedAt descending
                                  select new PurchaseOrderAdminInquiryViewModel
                                  {
                                      Id = inquiry.Id,
                                      Name = inquiry.Name,
                                      Phone = inquiry.Phone,
                                      Email = inquiry.Email,
                                      InquiryText = inquiry.InquiryText,
                                      PurchaseOrderId = inquiry.PurchaseOrderId,
                                      PurchaseOrderName = purchaseOrder.Title,
                                      AttachmentUrl = inquiry.AttachmentUrl, // رابط المرفق
                                      CreatedAt = inquiry.CreatedAt
                                  }).ToListAsync();

            return requests;
        }

        /// <summary>
        /// جلب بيانات طلب واحد بالتفصيل حسب Id مع اسم العطاء ورابط المرفق
        /// </summary>
        public async Task<PurchaseOrderAdminInquiryViewModel?> GetInquiryRequestByIdAsync(int id)
        {
            var request = await (from inquiry in _context.PurchaseOrderInquiries
                                 join purchaseOrder in _context.PurchaseOrders
                                     on inquiry.PurchaseOrderId equals purchaseOrder.Id
                                 where inquiry.Id == id
                                 select new PurchaseOrderAdminInquiryViewModel
                                 {
                                     Id = inquiry.Id,
                                     Name = inquiry.Name,
                                     Phone = inquiry.Phone,
                                     Email = inquiry.Email,
                                     InquiryText = inquiry.InquiryText,
                                     PurchaseOrderId = inquiry.PurchaseOrderId,
                                     PurchaseOrderName = purchaseOrder.Title,
                                     AttachmentUrl = inquiry.AttachmentUrl, // رابط المرفق
                                     CreatedAt = inquiry.CreatedAt
                                 }).FirstOrDefaultAsync();

            return request;
        }
    }
}
