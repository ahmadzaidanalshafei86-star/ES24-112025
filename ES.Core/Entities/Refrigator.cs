using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Core.Entities
{
    public class Refrigator
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        public ICollection<RefrigatorTranslate>? RefrigatorsTranslates { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!;
    }
}
