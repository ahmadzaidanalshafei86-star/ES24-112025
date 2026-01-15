using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace ES.Web.Areas.EsAdmin.Models
{
    public class PurchaseOrderTranslationFormViewModel
    {
        public int TranslationId { get; set; }
        public int PurchaseOrderId { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        public string Title { get; set; } = null!;

        public string? Details { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }

        [RequiredIf("TranslationId == 0", ErrorMessage = Errors.RequiredField)]
        public int? LanguageId { get; set; }
        public IEnumerable<SelectListItem>? Languages { get; set; }
    }
}
