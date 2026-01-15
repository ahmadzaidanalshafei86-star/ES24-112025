


using ES.Web.Models;
using ES.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ES.Web.Controllers
{
    public class RighttoobtaininformationController : Controller
    {
        private readonly RighttoobtaininformationService _righttoobtaininformationService;
        private readonly IStringLocalizer<RighttoobtaininformationController> _localizer;

        public RighttoobtaininformationController(RighttoobtaininformationService righttoobtaininformationService, IStringLocalizer<RighttoobtaininformationController> localizer)
        {
            _righttoobtaininformationService = righttoobtaininformationService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _righttoobtaininformationService.InitializeRighttoobtaininformationFormViewModel();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(RighttoobtaininformationFormViewModel model)
        {
            var refreshedModel = await _righttoobtaininformationService.InitializeRighttoobtaininformationFormViewModel();
            refreshedModel.Name = model.Name;

            try
            {
                // حفظ ملفات الرسالة
                var letterFileNames = new List<string>();
                if (model.LetterFiles != null)
                {
                    foreach (var file in model.LetterFiles)
                    {
                        var fileName = await _righttoobtaininformationService.SaveFileAsync(file);
                        if (fileName != null)
                            letterFileNames.Add(fileName);
                    }
                }

                // حفظ ملفات الهوية
                var personalIDFileNames = new List<string>();
                if (model.PersonalIdCopies != null)
                {
                    foreach (var file in model.PersonalIdCopies)
                    {
                        var fileName = await _righttoobtaininformationService.SaveFileAsync(file);
                        if (fileName != null)
                            personalIDFileNames.Add(fileName);
                    }
                }

                // حفظ المستندات الإضافية
                var additionalDocsFileNames = new List<string>();
                if (model.AdditionalDocuments != null)
                {
                    foreach (var file in model.AdditionalDocuments)
                    {
                        var fileName = await _righttoobtaininformationService.SaveFileAsync(file);
                        if (fileName != null)
                            additionalDocsFileNames.Add(fileName);
                    }
                }

                // إنشاء الطلب
                var request = new RighttoobtaininformationRequest
                {
                    ApplicantCategory = model.ApplicantCategory,
                    CompanyName = model.CompanyName,
                    AuthorizationBookNumber = model.AuthorizationBookNumber,
                    CommercialRegistrationNo = model.CommercialRegistrationNo,
                    AuthorizationLetterDate = model.AuthorizationLetterDate,
                    DelegateName = model.DelegateName,

                    Name = model.Name,
                    FatherName = model.FatherName,
                    GrandFatherName = model.GrandFatherName,
                    FamilyName = model.FamilyName,
                    NationalNumber = model.NationalNumber,

                    ResidenceGovernorate = model.ResidenceGovernorate,
                    ResidenceCity = model.ResidenceCity,
                    ResidenceDistrict = model.ResidenceDistrict,

                    WorkplaceGovernorate = model.WorkplaceGovernorate,
                    WorkplaceCity = model.WorkplaceCity,
                    Employer = model.Employer,

                    CellPhoneNumber = model.CellPhone,
                    LandlineNumber = model.Landline,
                    FaxNumber = model.Fax,
                    POBox = model.POBox,
                    Email = model.Email,

                    IdentificationType = model.IdentificationType,
                    OtherDocumentName = model.DocumentName,
                    InformationPurpose = model.Purpose,
                    OtherPurpose = model.OtherPurpose,
                    DeliveryMethod = model.DeliveryMethod,
                    OtherDeliveryMethod = model.OtherDelivery,

                    InformationTopic = model.InformationTopic,

                    Agreement = model.Agreement,
                    CreatedAt = DateTime.UtcNow
                };

                // حفظ الطلب + الملفات
                await _righttoobtaininformationService.SaveRighttoobtaininformationRequestAsync(
                    request,
                    letterFileNames,
                    personalIDFileNames,
                    additionalDocsFileNames
                );

                TempData["SuccessMessage"] = _localizer["Your Right to obtain information has been submitted successfully!"].Value;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = _localizer[$"An error occurred: {ex.Message}"].Value;
                return View("Index", refreshedModel);
            }
        }



    }

}
