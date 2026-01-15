using ES.Web.Areas.EsAdmin.Models;

namespace ES.Web.Models
{
    public class TenderDetailsViewModel
    {
        public int Id { get; set; }


        public string Title { get; set; } = null!;


        public string Slug { get; set; } = null!;


        public string Code { get; set; } = null!;


        public string CopyPrice { get; set; } = null!;

        // Dates
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

       
        public DateTime? EnvelopeOpeningDate { get; set; }
        public DateTime? LastCopyPurchaseDate { get; set; }
        public string MaterialName { get; set; } = null!;
        public int MaterialId { get; set; }

        // Flags
        public bool Publish { get; set; } = false;


        // 🔹 Move to archive (instead of deleting)
        public bool MoveToArchive { get; set; } = false;

        public string? InitialAwardFileUrl { get; set; }
        public string? FinalAwardFileUrl { get; set; }
        public string? PricesOfferedAttachmentUrl { get; set; }
        // 🔹 File Attachments
        public IList<IFormFile>? TenderFiles { get; set; } = new List<IFormFile>();
        public IList<TenderFilesViewModel>? ExistingFiles { get; set; } = new List<TenderFilesViewModel>();



    }
}
