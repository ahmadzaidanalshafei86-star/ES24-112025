using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace ES.Web.Models
{
    public class JobFormViewModel
    {
        public string? DeclerationName { get; set; } = string.Empty;
        public int DeclerationId { get; set; }
        [Required]
        public string NationalNumber { get; set; } = string.Empty;

        // English Names
        [Required]
        public string FirstNameEn { get; set; } = string.Empty;
        [Required]
        public string SecondNameEn { get; set; } = string.Empty;
        [Required]
        public string ThirdNameEn { get; set; } = string.Empty;
        [Required]
        public string FourthNameEn { get; set; } = string.Empty;

        // Arabic Names
        [Required]
        public string FirstNameAr { get; set; } = string.Empty;
        [Required]
        public string SecondNameAr { get; set; } = string.Empty;
        [Required]
        public string ThirdNameAr { get; set; } = string.Empty;
        [Required]
        public string FourthNameAr { get; set; } = string.Empty;

        // Nationality & place of birth
        [Required]
        public int SelectedNationalityId { get; set; }
        [Required]
        public int PlaceOfBirthId { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; } = new List<SelectListItem>();

        public int? SelectedStateId { get; set; }
        public IEnumerable<SelectListItem> States { get; set; } = new List<SelectListItem>();
   
        public int? SelectedCityId { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; } = new List<SelectListItem>();
        [Required]
        public int ReligionId { get; set; }
        [Required]
        public int GenderId { get; set; }
        [Required]
        public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Required]
        public int SelectedMaritalStatusId { get; set; }
        public IEnumerable<SelectListItem> MaritalStatuses { get; set; } = new List<SelectListItem>();

        [Required]
        public int SelectedBloodTypeId { get; set; }
        public IEnumerable<SelectListItem> BloodTypes { get; set; } = new List<SelectListItem>();


        [Required]
        public int AddressCountryId { get; set; }
        [Required]
        public int AddressStateId { get; set; }
        [Required]
        public int AddressCityId { get; set; }
        [Required]
        public string AddressText { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string? TelephoneNumber { get; set; }
        public string? PoBox { get; set; }
        [Required]
        public string MobileNumber { get; set; } = string.Empty;

        //Education
        [Required]
        public int UniversityCountryId{ get; set; }

        public int? SelectedUniversityId { get; set; }
        public IEnumerable<SelectListItem> Universities { get; set; } = new List<SelectListItem>();

        [Required]
        public int SelectedEducationDegreeId { get; set; }
        public IEnumerable<SelectListItem> EducationDegrees { get; set; } = new List<SelectListItem>();

        [Required]
        public int SelectedYearsOfStudyId { get; set; }
        public IEnumerable<SelectListItem> YearsOfStudy { get; set; } = new List<SelectListItem>();
        [Required]
        public int SelectedSpecializationId { get; set; }
        public IEnumerable<SelectListItem> Specializations { get; set; } = new List<SelectListItem>();

        public List<EmployeeLanguage> Languages { get; set; } = new List<EmployeeLanguage>();
        public List<EmployeeWorkHistory> WorkHistory { get; set; } = new List<EmployeeWorkHistory>();
        public List<EmployeeSkill> Skills { get; set; } = new List<EmployeeSkill>();
        public List<IFormFile> AttachmentFiles { get; set; } = new List<IFormFile>();

    }
}

