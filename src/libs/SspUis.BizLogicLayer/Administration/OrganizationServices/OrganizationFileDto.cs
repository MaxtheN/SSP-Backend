using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer
{
    public class OrganizationFileDto : OrganizationFileDlDto
    {
        public string FileName { get; internal set; }
        public DateTime CreatedAt { get; set; }
    }
}
