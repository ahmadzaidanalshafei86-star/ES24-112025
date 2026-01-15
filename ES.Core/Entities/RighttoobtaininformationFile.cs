using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Core.Entities
{
    public class RighttoobtaininformationFile
    {
        public int Id { get; set; }

        public int RequestId { get; set; }
        public RighttoobtaininformationRequest Request { get; set; }

        public string FileType { get; set; }  // LetterFile / PersonalIDCopy / AdditionalDocuments
        public string FileName { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
