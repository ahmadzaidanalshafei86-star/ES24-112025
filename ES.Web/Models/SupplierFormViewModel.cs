using System.ComponentModel.DataAnnotations;

namespace ES.Web.Models
{
    public class SupplierFormViewModel
    {

        [Required]
        public string UserType { get; set; } = null!; // selected from drop down (Individual OR Company)
        [Required]
        public string CompanyName { get; set; } = null!; 
        [Required]
        public string CompanySector { get; set; } = null!; // selected from drop down (Public or Provate)

        public string? RegistrationNumber { get; set; }

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string? EmailAddress { get; set; }

        public string? FaxNumber { get; set; }
        public string? WebsiteUrl { get; set; }

        public string? Address { get; set; }

        [Required]  
        public IFormFile CommercialRegister { get; set; } = null!;
        public IFormFile? ProfessionalLicense { get; set; }
        public IFormFile? ListOfKeyAchievements { get; set; }
        public IFormFile? ListOfMajorClients { get; set; }
        public IFormFile? CopyOfRegistrationCertificate { get; set; }



        public List<MaterialCheckboxViewModel> Materials { get; set; } = new();
    }
}
