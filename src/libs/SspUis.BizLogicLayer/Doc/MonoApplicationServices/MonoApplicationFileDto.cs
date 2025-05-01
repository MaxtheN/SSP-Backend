using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.MonoApplicationServices
{
    public class MonoApplicationFileDto : MonoApplicationFileDlDto
    {
        public DateTime CreatedAt { get; set; }
    }
}
