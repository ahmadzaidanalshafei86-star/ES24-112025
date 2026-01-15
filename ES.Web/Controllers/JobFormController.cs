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
            // تحقق من صحة النموذج
            if (!ModelState.IsValid)
            {
                // يمكن إرسال النموذج مع رسالة خطأ بدون إعادة تحميل الصفحة
                return Json(new
                {
                    success = false,
                    message = "Please fill all required fields.",
                    data = model // لإعادة ملء الحقول في الفورم إذا لزم
                });
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
                    // إعادة التوجيه عند النجاح
                    return Json(new
                    {
                        success = true,
                        redirectUrl = Url.Action("Success")
                    });
                }
                else
                {
                    // فشل في إرسال الطلب
                    return Json(new
                    {
                        success = false,
                        message = "Failed to submit your application. Please try again later.",
                        data = model
                    });
                }
            }
            catch (Exception ex)
            {
                // خطأ غير متوقع
                return Json(new
                {
                    success = false,
                    message = $"An unexpected error occurred: {ex.Message}",
                    data = model
                });
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
