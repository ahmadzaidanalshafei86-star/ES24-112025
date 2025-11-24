using ES.Web.Helpers;
using ES.Web.Models;

namespace ES.Web.Services
{
    public class JobFormService
    {
        private readonly ApplicationDbContext _context;
        public JobFormService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<JobFormViewModel> IntialzieJobFormViewModel(int declerationId)
        {
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);
            var decleration = await GetDeclerationNameById(declerationId);

            var model = new JobFormViewModel
            {
                DeclerationId = declerationId,
                DeclerationName = languageId == 1
                ? decleration?.Subject
                : decleration?.SubjectAr,
                Countries = JobFormLookUps.GetCountrySelectList(languageId),
                States = JobFormLookUps.GetStatesSelectList(1, languageId),
                Cities = JobFormLookUps.GetCitiesByCountryAndState(1, languageId),
                MaritalStatuses = JobFormLookUps.GetMaritalStatusSelectList(languageId),
                BloodTypes = JobFormLookUps.GetBloodTypesSelectList(languageId),
                EducationDegrees = JobFormLookUps.GetEducationDegreeSelectList(languageId),
                Universities = JobFormLookUps.GetUniversitiesByCountry(1, languageId),
                YearsOfStudy = JobFormLookUps.GetYearsOfStudySelectList(languageId),
                Specializations = JobFormLookUps.GetSpecializationsSelectList(languageId),
            };

            return model;
        }

      

        private async Task<SilosDeclerations?> GetDeclerationNameById(int declerationId)
        {
            return await _context.SilosDeclerations.FindAsync(declerationId);
        }
    }
}
