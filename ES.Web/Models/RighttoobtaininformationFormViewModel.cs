using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace ES.Web.Models
{
    public class RighttoobtaininformationFormViewModel
    {
        // Applicant Category
        [Required(ErrorMessage = "Please select applicant category")]
        public string ApplicantCategory { get; set; } // NormalPerson / LegalPerson

        // Private Sector (LegalPerson only)
        public string CompanyName { get; set; }               // [RequiredIf("ApplicantCategory", "LegalPerson")]
        public string AuthorizationBookNumber { get; set; }   // [RequiredIf("ApplicantCategory", "LegalPerson")]
        public string CommercialRegistrationNo { get; set; }  // [RequiredIf("ApplicantCategory", "LegalPerson")]
        public DateTime? AuthorizationLetterDate { get; set; }// [RequiredIf("ApplicantCategory", "LegalPerson")]
        public string DelegateName { get; set; }              // [RequiredIf("ApplicantCategory", "LegalPerson")]

        // Multiple files support
        public IFormFile[] LetterFiles { get; set; }          // The letter of the concerned authority
        public IFormFile[] PersonalIdCopies { get; set; }     // Personal identification copy
        public IFormFile[] AdditionalDocuments { get; set; }  // Additional documents

        // Personal Info
        [Required] public string Name { get; set; }
        [Required] public string FatherName { get; set; }
        [Required] public string GrandFatherName { get; set; }
        [Required] public string FamilyName { get; set; }
        [Required] public string NationalNumber { get; set; }

        // Residence
        public string ResidenceGovernorate { get; set; }
        public string ResidenceCity { get; set; }
        public string ResidenceDistrict { get; set; }

        // Workplace
        public string WorkplaceGovernorate { get; set; }
        public string WorkplaceCity { get; set; }
        public string Employer { get; set; }

        // Contact
        public string CellPhone { get; set; }
        public string Landline { get; set; }
        public string Fax { get; set; }
        public string POBox { get; set; }
        [Required] public string Email { get; set; }

        // Identification type
        [Required] public string IdentificationType { get; set; } // IDCard / Passport / Other
        public string DocumentName { get; set; }                  // Required if IdentificationType == Other

        // Purpose
        [Required] public string Purpose { get; set; }           // Studies / Publishing / Other
        public string OtherPurpose { get; set; }                 // Required if Purpose == Other

        // Delivery method
        [Required] public string DeliveryMethod { get; set; }    // Photocopy / CDROM / Other
        public string OtherDelivery { get; set; }                // Required if DeliveryMethod == Other

        // Information
        [Required] public string InformationTopic { get; set; }

        // Agreement
        [Required(ErrorMessage = "You must agree")]
        public bool Agreement { get; set; }
    }
}
