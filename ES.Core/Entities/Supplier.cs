using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Core.Entities
{
    public class Supplier
    {
        public int Id { get; set; }

        public string UserType { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string CompanySector { get; set; } = null!;
        public string? RegistrationNumber { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string? FaxNumber { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Address { get; set; }

        // File names saved in DB
        public string CommercialRegisterPath { get; set; } = null!;
        public string? ProfessionalLicensePath { get; set; }
        public string? ListOfKeyAchievementsPath { get; set; }
        public string? ListOfMajorClientsPath { get; set; }
        public string? CopyOfRegistrationCertificatePath { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<SupplierMaterial> SupplierMaterials { get; set; } = new List<SupplierMaterial>();
    }

}
