


using AKM.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ES.Core.Entities
{
    [Index(nameof(Code), IsUnique = true)]
    public class PurchaseOrder
    {
        public int Id { get; set; }

        [Required, MaxLength(300)]
        public string Title { get; set; } = null!;

        [Required, MaxLength(300)]
        public string Slug { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Code { get; set; } = null!;

        [Required, MaxLength(50)]
       

        // Dates
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? EnvelopeOpeningDate { get; set; }

        public string? Details { get; set; }

        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }


        public Material? Material { get; set; }

        // PurchaseOrder materials (Many-to-Many)
        public ICollection<PurchaseOrderMaterial> Materials { get; set; } = new List<PurchaseOrderMaterial>();

        // PurchaseOrder files (multiple)
        public ICollection<PurchaseOrderFile> PurchaseOrderFiles { get; set; } = new List<PurchaseOrderFile>();

       
        // Attachments / Files
        public string? PurchaseOrderImageUrl { get; set; }
        public string? PurchaseOrderImageAltName { get; set; }


        public string Numberofparticipatingcompanies { get; set; } = null!;
        public string Thenumberofcompaniesreferredto { get; set; } = null!;



        // Flags
        public bool Publish { get; set; } = false;

        // 🔹 Move to archive (instead of deleting)
        public bool MoveToArchive { get; set; } = false;

       

        // Timestamps
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }

        // Translations
        public int LanguageId { get; set; }
        public Language Language { get; set; } = null!;

        public ICollection<PurchaseOrderTranslate>? PurchaseOrderTranslates { get; set; }
    }

    // 🔹 Junction table for PurchaseOrder ↔ Material (Many-to-Many)
    public class PurchaseOrderMaterial
    {
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public int MaterialId { get; set; }
        public Material Material { get; set; } = null!;
    }

    // 🔹 Multiple PurchaseOrder files (e.g., specifications, documents)
    public class PurchaseOrderFile
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string AltName { get; set; } = null!;
        [MaxLength(255)]
        public string FileUrl { get; set; } = null!;
        public int DisplayOrder { get; set; }

        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

   
}

