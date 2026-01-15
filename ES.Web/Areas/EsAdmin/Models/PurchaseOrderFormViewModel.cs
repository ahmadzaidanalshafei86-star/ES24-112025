using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ES.Web.Areas.EsAdmin.Models
{
    public class PurchaseOrderFormViewModel
    {
        public int? Id { get; set; }

        // 🔹 Basic Info
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(300)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Slug { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [MaxLength(100)]
        public string Code { get; set; } = string.Empty;

 

        // 🔹 Dates
        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        public DateTime EndDate { get; set; }

        public DateTime? EnvelopeOpeningDate { get; set; }



        public string Numberofparticipatingcompanies { get; set; } = string.Empty;
        public string Thenumberofcompaniesreferredto { get; set; } = string.Empty;

        // 🔹 Content
        public string? Details { get; set; }
     
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }

        // 🔹 Materials
        public List<int>? SelectedMaterialIds { get; set; } = new();
        public IEnumerable<SelectListItem>? Materials { get; set; }

        // 🔹 File Attachments
        public IList<IFormFile>? PurchaseOrderFiles { get; set; } = new List<IFormFile>();
        public IList<PurchaseOrderFileViewModel>? ExistingFiles { get; set; } = new List<PurchaseOrderFileViewModel>();

        // 🔹 Track removed existing files (Dropzone)
        public List<int>? RemovedFiles { get; set; } = new List<int>();

        // 🔹 Other Attachments
       
        // 🔹 Uploads
        public IFormFile? PurchaseOrderImage { get; set; }
        public string? PurchaseOrderImageUrl { get; set; }

       

  

        // 🔹 Flags
        public bool Publish { get; set; }

        public bool MoveToArchive { get; set; }

      
       

        // 🔹 Language
        public int? LanguageId { get; set; }
        public IEnumerable<SelectListItem>? Languages { get; set; }

        // 🔹 Dropdown Data
        public IEnumerable<SelectListItem>? PurchaseOrders { get; set; }
        public IEnumerable<SelectListItem>? SortingTypes { get; set; }

        // 🔹 System Fields
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public int Count { get; set; }

        // 🔹 Extra Attachments
        public List<PurchaseOrderOtherAttachmentViewModel> PurchaseOrderOtherAttachments { get; set; } = new();
    }

    public class PurchaseOrderFileViewModel
    {
        public int? Id { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class PurchaseOrderOtherAttachmentViewModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public IFormFile? File { get; set; }
        public string? FileUrl { get; set; }
    }
}
