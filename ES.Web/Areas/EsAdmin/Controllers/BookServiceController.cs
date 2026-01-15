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
    public class BookServiceController : Controller
    {
        private readonly BookServiceRepository _bookserviceRepository;
        private readonly RoleManager<IdentityRole> _roleManager;

        public BookServiceController(
            BookServiceRepository bookserviceRepository,
            RoleManager<IdentityRole> roleManager)
        {
            _bookserviceRepository = bookserviceRepository;
            _roleManager = roleManager;
        }

        [Authorize(Permissions.Materials.Read)]
        public async Task<IActionResult> Index()
        {
            // جلب كل طلبات BookService من قاعدة البيانات
            var bookServiceRequests = await _bookserviceRepository.GetBookServiceInfoAsync();

            // عرض البيانات في View
            return View(bookServiceRequests);
        }

        [Authorize(Permissions.Materials.Read)]
        public async Task<IActionResult> Details(int id)
        {
            // جلب بيانات طلب محدد
            var request = await _bookserviceRepository.GetBookServiceRequestByIdAsync(id);

            if (request == null)
                return NotFound();

            return View(request);
        }


        [HttpPost]
        [Authorize(Permissions.Materials.Read)]
        public async Task<IActionResult> ExportToExcel()
        {
            var requests = await _bookserviceRepository.GetBookServiceInfoAsync();

            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var worksheet = workbook.Worksheets.Add("BookService");

            // Header
            worksheet.Cell(1, 1).Value = "Name";
            worksheet.Cell(1, 2).Value = "Phone";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Branch";
            worksheet.Cell(1, 5).Value = "Hangars";
            worksheet.Cell(1, 6).Value = "Refrigators";
            worksheet.Cell(1, 7).Value = "Notes";
            worksheet.Cell(1, 8).Value = "Created At";

            int row = 2;
            foreach (var r in requests)
            {
                worksheet.Cell(row, 1).Value = r.Name ?? "";
                worksheet.Cell(row, 2).Value = r.PhoneNumber ?? "";
                worksheet.Cell(row, 3).Value = r.EmailAddress ?? "";
                worksheet.Cell(row, 4).Value = r.BranchName ?? "";
                worksheet.Cell(row, 5).Value = (r.Hangars != null && r.Hangars.Any()) ? string.Join(", ", r.Hangars) : "";
                worksheet.Cell(row, 6).Value = (r.Refrigators != null && r.Refrigators.Any()) ? string.Join(", ", r.Refrigators) : "";
                worksheet.Cell(row, 7).Value = r.Notes ?? "";
                worksheet.Cell(row, 8).Value = r.CreatedAt.ToString("yyyy-MM-dd HH:mm");
                row++;
            }

            worksheet.Columns().AdjustToContents();

            // إنشاء Stream بدون استخدام 'using'
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"BookService_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }



    }
}
