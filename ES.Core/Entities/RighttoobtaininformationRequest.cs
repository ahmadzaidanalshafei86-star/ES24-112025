using System;

namespace ES.Core.Entities
{
    public class RighttoobtaininformationRequest
    {
        public int Id { get; set; }

        // Applicant Category
        public string? ApplicantCategory { get; set; }

        // Private Sector
        public string? CompanyName { get; set; }
        public string? AuthorizationBookNumber { get; set; }
        public string? CommercialRegistrationNo { get; set; }
        public DateTime? AuthorizationLetterDate { get; set; }
        public string? DelegateName { get; set; }
        public string? LetterFileName { get; set; }

        // Personal Info
        public string? Name { get; set; }
        public string? FatherName { get; set; }
        public string? GrandFatherName { get; set; }
        public string? FamilyName { get; set; }
        public string? NationalNumber { get; set; }
        public string? PersonalIDCopyFileName { get; set; }

        // Residence
        public string? ResidenceGovernorate { get; set; }
        public string? ResidenceCity { get; set; }
        public string? ResidenceDistrict { get; set; }

        // Workplace
        public string? WorkplaceGovernorate { get; set; }
        public string? WorkplaceCity { get; set; }
        public string? Employer { get; set; }

        // Contact
        public string? CellPhoneNumber { get; set; }
        public string? LandlineNumber { get; set; }
        public string? FaxNumber { get; set; }
        public string? POBox { get; set; }
        public string? Email { get; set; }

        // Identification & purpose
        public string? IdentificationType { get; set; }
        public string? OtherDocumentName { get; set; }
        public string? InformationPurpose { get; set; }
        public string? OtherPurpose { get; set; }
        public string? DeliveryMethod { get; set; }
        public string? OtherDeliveryMethod { get; set; }

        // Information topic
        public string? InformationTopic { get; set; }
        public string? AdditionalDocumentsFileName { get; set; }

        // Agreement
        public bool? Agreement { get; set; }

        // Created date
        public DateTime? CreatedAt { get; set; }
        // Navigation property
        public ICollection<RighttoobtaininformationFile> RighttoobtaininformationFiles { get; set; } = new List<RighttoobtaininformationFile>();
    }
}
