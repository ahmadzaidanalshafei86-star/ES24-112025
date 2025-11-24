using ES.Web.Models;
using System.Globalization;

namespace ES.Web.Services
{
    public class SilosJobRequestMapper
    {
        public SilosJobRequestDto MapJobFormViewModelToSilosRequest(JobFormViewModel model, int jobCode, int declerationNumber)
        {

            // Helper function to clean phone numbers
            long ParsePhoneNumber(string? number)
            {
                if (string.IsNullOrWhiteSpace(number)) return 0;
                // Keep only digits
                var digitsOnly = new string(number.Where(char.IsDigit).ToArray());
                return long.TryParse(digitsOnly, out var result) ? result : 0;
            }


            var dto = new SilosJobRequestDto
            {
                DECLERATION_NO = declerationNumber.ToString(),
                JOB_CODE = jobCode,
                NATIONAL_NO = model.NationalNumber,
                NAM_A1 = model.FirstNameAr,
                NAM_A2 = model.SecondNameAr,
                NAM_A3 = model.ThirdNameAr,
                NAM_A4 = model.FourthNameAr,
                NAME_E1 = model.FirstNameEn,
                NAME_E2 = model.SecondNameEn,
                NAME_E3 = model.ThirdNameEn,
                NAME_E4 = model.FourthNameEn,
                CODE_NATIONALITY = model.SelectedNationalityId,
                PLACE_OF_BIRTH = model.PlaceOfBirthId.ToString(),
                COUNTRY_CODE_BIRTH_PLACE = 1, // You may need to map this from model
                STATE_CODE_BIRTH_PLACE = model.SelectedStateId ?? 0,
                CITY_CODE_BIRTH_PLACE = model.SelectedCityId ?? 0,
                RELIGION_ID = model.ReligionId,
                GENDER = model.GenderId,
                DATE_OF_BIRTH = model.DateOfBirth.ToString("yyyy-MM-dd"),
                PASSPORT_NO = "", // Not in current model
                PASSPORT_ISSUE_DATE = "", // Not in current model
                PASSPORT_EXPIRE_DATE = "", // Not in current model
                DATE_AVAILABLE_TO_WORK = DateTime.Today.ToString("yyyy-MM-dd"),
                MARITAL_STATUS = model.SelectedMaritalStatusId,
                BLOOD_TYPE = model.SelectedBloodTypeId,
                COUNTRY_CODE_ADDRESS = model.AddressCountryId.ToString(),
                STATE_CODE_ADDRESS = model.AddressStateId.ToString(),
                CITY_CODE_ADDRESS = model.AddressCityId.ToString(),
                ADDRESS = model.AddressText,
                EMAIL = model.Email,
                TELEPHONE_NO = ParsePhoneNumber(model.TelephoneNumber),
                PO_BOX = model.PoBox ?? "",
                MOBILE = ParsePhoneNumber(model.MobileNumber),
                NO_OF_CHILDREN = 0, // Not in current model
                EXPECTED_SALARY = 0, // Not in current model
                SOCIAL_SECURITY_NO = 0, // Not in current model
                EDUCATION_INFO = MapEducationInfo(model),
                EMPLOYEE_LANGUAGES = MapLanguagesInfo(model),
                EXPERIENCE_INFO = MapExperienceInfo(model),
                SKILLS_INFO = MapSkillsInfo(model)
            };

            return dto;
        }

        private List<EducationInfoDto> MapEducationInfo(JobFormViewModel model)
        {
            var educationList = new List<EducationInfoDto>();

            // Assuming only one education entry for now based on the JobFormViewModel structure
            if (model.SelectedUniversityId.HasValue)
            {
                educationList.Add(new EducationInfoDto
                {
                    UNIV_COUNTRY_CODE = model.UniversityCountryId.ToString(),
                    UNV_CODE = model.SelectedUniversityId!.Value.ToString(),
                    SPECIAL_CODE = model.SelectedSpecializationId.ToString(),
                    YEARS_OF_STUDY = model.SelectedYearsOfStudyId.ToString(),
                    EDU_DEGREE = model.SelectedEducationDegreeId.ToString(),
                    START_DATE = DateTime.Today.ToString("yyyy-MM-dd"), // Not in model
                    END_DATE = DateTime.Today.ToString("yyyy-MM-dd"), // Not in model
                    GPA = "0" // Not in model
                });
            }

            return educationList;
        }

        private List<LanguageInfoDto> MapLanguagesInfo(JobFormViewModel model)
        {
            var languagesList = new List<LanguageInfoDto>();

            foreach (var lang in model.Languages ?? new List<EmployeeLanguage>())
            {
                languagesList.Add(new LanguageInfoDto
                {
                    LANGUAGE_ID = lang.LanguageId.ToString(),
                    READING_LEVEL = ConvertLevelToGrade(lang.ReadingLevel),
                    WRITING_LEVEL = ConvertLevelToGrade(lang.WritingLevel),
                    SPEAKING_LEVEL = ConvertLevelToGrade(lang.SpeakingLevel)
                });
            }

            return languagesList;
        }

        private List<ExperienceInfoDto> MapExperienceInfo(JobFormViewModel model)
        {
            var experienceList = new List<ExperienceInfoDto>();

            foreach (var work in model.WorkHistory ?? new List<EmployeeWorkHistory>())
            {
                experienceList.Add(new ExperienceInfoDto
                {
                    COMPANY_NAME = work.CompanyName,
                    COMPANY_ADDRESS = work.CompanyAddress,
                    START_DATE = work.StartDate?.ToString("yyyy-MM-dd") ?? "",
                    END_DATE = work.EndDate?.ToString("yyyy-MM-dd") ?? "",
                    LAST_SALARY = work.LastSalary ?? 0,
                    MAIN_JOB_DESCRIPTION = work.MainJobDescription,
                    DIRECT_MANAGER_NAME = work.DirectManagerName,
                    LEAVE_REASON = work.LeaveReason
                });
            }

            return experienceList;
        }

        private List<SkillInfoDto> MapSkillsInfo(JobFormViewModel model)
        {
            var skillsList = new List<SkillInfoDto>();

            foreach (var skill in model.Skills ?? new List<EmployeeSkill>())
            {
                skillsList.Add(new SkillInfoDto
                {
                    SKILL_DESCRIPTION = skill.SkillDescription,
                    SKILL_LEVEL = skill.SkillLevel.ToString(),
                    SKILL_MAIN_TYPE = "1", // Not in model
                    SKILL_SUB_TYPE = "2"  // Not in model
                });
            }

            return skillsList;
        }

        private string ConvertLevelToGrade(int level)
        {
            return level switch
            {
                1 => "B", // Basic
                2 => "I", // Intermediate
                3 => "E", // Expert/Advanced
                _ => "B"
            };
        }
    }
}
