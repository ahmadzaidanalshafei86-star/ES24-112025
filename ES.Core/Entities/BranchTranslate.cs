

namespace ES.Core.Entities
{
    public class BranchTranslate
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!;
        public int? LanguageId { get; set; }
        public Language? Language { get; set; }
    }
}

