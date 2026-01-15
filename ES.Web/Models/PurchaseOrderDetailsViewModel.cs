using ES.Web.Areas.EsAdmin.Models;

namespace ES.Web.Models
{
    public class PurchaseOrderDetailsViewModel
    {
        public int Id { get; set; }

        // Basic Info
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Code { get; set; } = null!;

        public string Numberofparticipatingcompanies { get; set; } = null!;

        public string Thenumberofcompaniesreferredto { get; set; } = null!;

        // Dates
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateTime? EnvelopeOpeningDate { get; set; }

        // Material
        public string MaterialName { get; set; } = null!;
        public int MaterialId { get; set; }

        // Flags
        public bool Publish { get; set; } = false;

        // Move to archive (instead of deleting)
        public bool MoveToArchive { get; set; } = false;

      

        // File Attachments (Uploads)
        public IList<IFormFile>? PurchaseOrderFiles { get; set; } = new List<IFormFile>();

        // Existing Saved Files
        public IList<PurchaseOrderFilesViewModel>? ExistingFiles { get; set; } = new List<PurchaseOrderFilesViewModel>();
    }
}
