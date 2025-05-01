using SspUis.DataLayer.Repositories.Corruption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.JoinAntiCorruptionResultServices
{
    public class JoinAntiCorruptionResultFileDto : JoinAntiCorruptionResultFileDlDto
    {
        public string FileName { get; internal set; }
        public DateTime CreatedAt { get; set; }
    }
}
