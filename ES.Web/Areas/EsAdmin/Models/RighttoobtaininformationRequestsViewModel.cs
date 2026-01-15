namespace ES.Web.Areas.EsAdmin.Models
{
    public class RighttoobtaininformationRequestsViewModel
    {


        public int Id { get; set; }

        public string ApplicantCategory { get; set; } // NormalPerson / LegalPerson


        // Personal Info
        public string Name { get; set; }
         public string FatherName { get; set; }
         public string GrandFatherName { get; set; }
         public string FamilyName { get; set; }
        public string NationalNumber { get; set; }


        // Contact
        public string CellPhone { get; set; }
        public string Landline { get; set; }
        public string Fax { get; set; }
        public string POBox { get; set; }
        public string Email { get; set; }

        public DateTime? CreatedAt { get; set; }





        // Private Sector (LegalPerson only)
        // These fields are required only if ApplicantCategory == "LegalPerson"
        public string CompanyName { get; set; }               // [RequiredIf("ApplicantCategory", "LegalPerson")]
        public string AuthorizationBookNumber { get; set; }   // [RequiredIf("ApplicantCategory", "LegalPerson")]
        public string CommercialRegistrationNo { get; set; }  // [RequiredIf("ApplicantCategory", "LegalPerson")]
        public DateTime? AuthorizationLetterDate { get; set; }// [RequiredIf("ApplicantCategory", "LegalPerson")]
        public string DelegateName { get; set; }              // [RequiredIf("ApplicantCategory", "LegalPerson")]
        public IFormFile LetterFile { get; set; }             // Optional

       
        public IFormFile PersonalIdCopy { get; set; }

        // Residence
        public string ResidenceGovernorate { get; set; }
        public string ResidenceCity { get; set; }
        public string ResidenceDistrict { get; set; }

        // Workplace
        public string WorkplaceGovernorate { get; set; }
        public string WorkplaceCity { get; set; }
        public string Employer { get; set; }


        // Identification type
        public string IdentificationType { get; set; } // IDCard / Passport / Other
        public string DocumentName { get; set; }                  // Required if IdentificationType == Other

        // Purpose
        public string Purpose { get; set; }           // Studies / Publishing / Other
        public string OtherPurpose { get; set; }                 // Required if Purpose == Other

        // Delivery method
         public string DeliveryMethod { get; set; }    // Photocopy / CDROM / Other
        public string OtherDelivery { get; set; }                // Required if DeliveryMethod == Other

        // Information
         public string InformationTopic { get; set; }
        public IFormFile AdditionalDocuments { get; set; }

        
        public bool Agreement { get; set; }
        // Files
        public List<RighttoobtaininformationFileViewModel> Files { get; set; } = new();

    }

    public class RighttoobtaininformationFileViewModel
    {
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
