using Microsoft.AspNetCore.Mvc.Rendering;
using ES.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ES.Web.Areas.EsAdmin.Models
{
    public class BranchFormViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = Errors.RequiredField)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = Errors.RequiredField)]
        public TypeOfSorting TypeOfSorting { get; set; }
        public IEnumerable<SelectListItem>? SortingTypes { get; set; }

        public int Order { get; set; } = 0;

        public IEnumerable<SelectListItem>? Branches { get; set; }

        // 🔹 Multi Hangars
        public List<HangarDto> Hangars { get; set; } = new();

        // 🔹 Multi Refrigators
        public List<RefrigatorDto> Refrigators { get; set; } = new();

        // 🔹 Supported Languages
        public IEnumerable<SelectListItem> Languages { get; set; } = new List<SelectListItem>();
    }

    public class HangarDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Size { get; set; } = "";
        public string Type { get; set; } = "";

        // 🔹 Translations
        public List<HangarTranslateDto> Translates { get; set; } = new();
    }

    public class RefrigatorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Size { get; set; } = "";
        public string Type { get; set; } = "";

        // 🔹 Translations
        public List<RefrigatorTranslateDto> Translates { get; set; } = new();
    }

    public class HangarTranslateDto
    {
        public string Name { get; set; } = "";
        public string Size { get; set; } = "";
        public string Type { get; set; } = "";
        public int? LanguageId { get; set; }
    }

    public class RefrigatorTranslateDto
    {
        public string Name { get; set; } = "";
        public string Size { get; set; } = "";
        public string Type { get; set; } = "";
        public int? LanguageId { get; set; }
    }
}
