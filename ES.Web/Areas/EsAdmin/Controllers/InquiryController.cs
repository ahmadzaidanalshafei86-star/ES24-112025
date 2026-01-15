



using ES.Core.Entities;
using ES.Web.Areas.EsAdmin.Models;
using ES.Web.Areas.EsAdmin.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ES.Web.Areas.EsAdmin.Controllers
{
    [Area("EsAdmin")]
    [Authorize]
    public class InquiryController : Controller
    {
        private readonly InquiryRepository _InquiryRepository;
        private readonly RoleManager<IdentityRole> _roleManager;

        public InquiryController(
            InquiryRepository InquiryRepository,
            RoleManager<IdentityRole> roleManager)
        {
            _InquiryRepository = InquiryRepository;
            _roleManager = roleManager;
        }

        [Authorize(Permissions.Materials.Read)]
        public async Task<IActionResult> Index()
        {
            // جلب كل طلبات Inquiry من قاعدة البيانات
            var InquiryRequests = await _InquiryRepository.GetInquiryInfoAsync();

            // عرض البيانات في View
            return View(InquiryRequests);
        }

        [Authorize(Permissions.Materials.Read)]
        public async Task<IActionResult> Details(int id)
        {
            // جلب بيانات طلب محدد
            var request = await _InquiryRepository.GetInquiryRequestByIdAsync(id);

            if (request == null)
                return NotFound();

            return View(request);
        }


        [HttpPost]
        [Authorize(Permissions.Materials.Read)]
        public async Task<IActionResult> ExportToExcel()
        {
            var requests = await _InquiryRepository.GetInquiryInfoAsync();

            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Inquiry");

            // Header
            worksheet.Cell(1, 1).Value = "Name";
            worksheet.Cell(1, 2).Value = "Phone";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Tender";
       
            worksheet.Cell(1, 5).Value = "Inquiry";
            worksheet.Cell(1, 6).Value = "Created At";

            int row = 2;
            foreach (var r in requests)
            {
                worksheet.Cell(row, 1).Value = r.Name ?? "";
                worksheet.Cell(row, 2).Value = r.Phone ?? "";
                worksheet.Cell(row, 3).Value = r.Email ?? "";
                worksheet.Cell(row, 4).Value = r.TenderName ?? "";
                worksheet.Cell(row, 5).Value = r.InquiryText ?? "";
                worksheet.Cell(row, 6).Value = r.CreatedAt.ToString("yyyy-MM-dd HH:mm");
                row++;
            }

            worksheet.Columns().AdjustToContents();

            // إنشاء Stream بدون استخدام 'using'
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"Inquiry_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }



    }
}

