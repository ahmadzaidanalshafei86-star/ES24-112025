




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
    public class RighttoobtaininformationRequestsController : Controller
    {
        private readonly RighttoobtaininformationRequestsRepository _RighttoobtaininformationRequestsRepository;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RighttoobtaininformationRequestsController(
            RighttoobtaininformationRequestsRepository RighttoobtaininformationRequestsRepository,
            RoleManager<IdentityRole> roleManager)
        {
            _RighttoobtaininformationRequestsRepository = RighttoobtaininformationRequestsRepository;
            _roleManager = roleManager;
        }

        [Authorize(Permissions.Materials.Read)]
        public async Task<IActionResult> Index()
        {
            // جلب كل طلبات RighttoobtaininformationRequests من قاعدة البيانات
            var RighttoobtaininformationRequestsRequests = await _RighttoobtaininformationRequestsRepository.GetRighttoobtaininformationRequestsInfoAsync();

            // عرض البيانات في View
            return View(RighttoobtaininformationRequestsRequests);
        }

        [Authorize(Permissions.Materials.Read)]
        public async Task<IActionResult> Details(int id)
        {
            // جلب بيانات طلب محدد
            var request = await _RighttoobtaininformationRequestsRepository.GetRighttoobtaininformationRequestsByIdAsync(id);

            if (request == null)
                return NotFound();

            return View(request);
        }


      



    }
}

