using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Core.Entities
{
    public class TenderInquiry
    {

        public int Id { get; set; }
        public int TenderId { get; set; }  // فقط العمود الصحيح

        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string InquiryText { get; set; } = null!;
        public string? AttachmentUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
