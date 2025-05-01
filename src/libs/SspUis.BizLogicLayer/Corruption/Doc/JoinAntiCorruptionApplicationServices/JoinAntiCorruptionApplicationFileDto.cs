using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.JoinAntiCorruptionApplicationServices
{
    public class JoinAntiCorruptionApplicationFileDto : JoinAntiCorruptionApplicationFileDlDto
    {
        public string FileName { get; internal set; }
        public DateTime CreatedAt { get; set; }
    }
}
