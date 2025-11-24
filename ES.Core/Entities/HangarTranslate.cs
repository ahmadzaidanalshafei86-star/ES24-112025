using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Core.Entities
{
    public class HangarTranslate
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Size { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int HangarId { get; set; }
        public Hangar Hangar { get; set; } = null!;
        public int? LanguageId { get; set; }
        public Language? Language { get; set; }


    }
}
