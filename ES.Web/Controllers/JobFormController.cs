using ES.Web.Helpers;
using ES.Web.Models;
using ES.Web.Services;

namespace ES.Web.Controllers
{
    public class JobFormController : Controller
    {
        private readonly JobFormService _jobFormService;
        private readonly ApplicationDbContext _context;
        private readonly SilosApiService _silosApiService;
        private readonly SilosJobRequestMapper _silosMapper;

        public JobFormController(
            JobFormService jobFormService,
            ApplicationDbContext context,
            SilosApiService silosApiService,
            SilosJobRequestMapper silosMapper)
        {
            _jobFormService = jobFormService;
            _context = context;
            _silosApiService = silosApiService;
            _silosMapper = silosMapper;
        }

        public async Task<IActionResult> Index(int declerationId)
        {
            var model = await _jobFormService.IntialzieJobFormViewModel(declerationId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(JobFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var initializedModel = await _jobFormService.IntialzieJobFormViewModel(model.DeclerationId);
                return View("Index", initializedModel);
            }

            try
            {
                // تحويل بيانات النموذج إلى صيغة SILOS API
                var jobCode = 99; // Default value - يمكن تغييره حسب الحاجة
                var silosRequest = _silosMapper.MapJobFormViewModelToSilosRequest(model, jobCode, model.DeclerationId);

                // إرسال الطلب إلى SILOS API
                var isSuccess = await _silosApiService.PostJobRequestAsync(silosRequest);

                if (isSuccess)
                {
                    // إعادة التوجيه إلى صفحة نجاح
                    return RedirectToAction("Success");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to submit your application. Please try again later.";
                    var initializedModel = await _jobFormService.IntialzieJobFormViewModel(model.DeclerationId);
                    return View("Index", initializedModel);
                }
            }
            catch (Exception ex)
            {
                // في حال حدوث أي خطأ غير متوقع
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                var initializedModel = await _jobFormService.IntialzieJobFormViewModel(model.DeclerationId);
                return View("Index", initializedModel);
            }
        }

        // صفحة جديدة تعرض رسالة النجاح
        [HttpGet]
        public IActionResult Success()
        {
            ViewBag.Message = "Your job application has been submitted successfully!";
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetCitiesByCountryAndState(int stateId)
        {
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);
            var cities = JobFormLookUps.GetCitiesByCountryAndState(stateId, languageId);
            return Json(cities);
        }

        [HttpGet]
        public async Task<IActionResult> GetUniversitiesByCountry(int countryId)
        {
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);
            var universities = JobFormLookUps.GetUniversitiesByCountry(countryId, languageId)
                .Select(u => new { id = u.Value, text = u.Text });

            return Json(universities);
        }

    }
}
