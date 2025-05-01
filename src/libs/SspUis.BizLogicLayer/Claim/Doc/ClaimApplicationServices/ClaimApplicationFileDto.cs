using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.ClaimApplicationServices
{
    public class ClaimApplicationFileDto : ClaimApplicationFileDlDto
    {
        public string FileName { get; internal set; }
        public DateTime CreatedAt { get; set; }
    }
}