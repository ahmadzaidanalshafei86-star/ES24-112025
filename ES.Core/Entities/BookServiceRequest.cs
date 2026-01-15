using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Core.Entities
{
    public class BookServiceRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? EmailAddress { get; set; }
        public string? Notes { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!; // <-- هذا الإضافة
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<BookServiceHangar>? Hangars { get; set; }
        public ICollection<BookServiceRefrigator>? Refrigators { get; set; }
    }

    public class BookServiceHangar
    {
        public int Id { get; set; }
        public int HangarId { get; set; }
        public string Name { get; set; } = null!;
        public int BookServiceRequestId { get; set; }
        public BookServiceRequest BookServiceRequest { get; set; } = null!;
    }

    public class BookServiceRefrigator
    {
        public int Id { get; set; }
        public int RefrigatorId { get; set; }
        public string Name { get; set; } = null!;
        public int BookServiceRequestId { get; set; }
        public BookServiceRequest BookServiceRequest { get; set; } = null!;
    }

}
