using System.ComponentModel.DataAnnotations;

namespace ES.Web.Models
{
    public class EmployeeWorkHistory
    {
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyAddress { get; set; } = string.Empty;
        public string PositionDescription { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DirectManagerName { get; set; } = string.Empty;
        public decimal? LastSalary { get; set; }
        public string MainJobDescription { get; set; } = string.Empty;
        public string LeaveReason { get; set; } = string.Empty;
    }
}
