using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer
{
    public class NeedChamberServiceFileDto : NeedChamberServiceFileDlDto
    {
        public string FileName { get; internal set; }
        public DateTime CreatedAt { get; set; }
    }
}
