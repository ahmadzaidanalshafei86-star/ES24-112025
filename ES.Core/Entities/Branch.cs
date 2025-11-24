

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Core.Entities
{
    [Index(nameof(Slug), IsUnique = true)]
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public int LanguageId { get; set; }
        public Language Language { get; set; } = null!;
        public ICollection<BranchTranslate>? BranchesTranslates { get; set; }

        public ICollection<Hangar>? RelatedHangars { get; set; }
        public ICollection<Refrigator>? RelatedRefrigators { get; set; }
    }
}
