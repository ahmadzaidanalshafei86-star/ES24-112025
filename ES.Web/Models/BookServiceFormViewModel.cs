using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ES.Web.Models
{
    public class BookServiceFormViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? EmailAddress { get; set; }

        public string? Notes { get; set; }

        [Required(ErrorMessage = "Branch selection is required")]
        public int? BranchId { get; set; }

        // Branch list for dropdown
        public List<BranchSelectViewModel> Branches { get; set; } = new();

        // Hangars checklist
        public List<HangarCheckboxViewModel> Hangars { get; set; } = new();

        // Refrigators checklist
        public List<RefrigatorCheckboxViewModel> Refrigators { get; set; } = new();
    }

    // Hangar checkbox item
    public class HangarCheckboxViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsSelected { get; set; }
    }

    // Refrigator checkbox item
    public class RefrigatorCheckboxViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsSelected { get; set; }
    }

    // Dropdown branch item
    public class BranchSelectViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
