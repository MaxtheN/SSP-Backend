using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer
{
    public class SrvApplicationTableFileDto : ServiceApplicationTableFileDlDto,
        ILinkToEntity<ServiceApplicationTableFile>
    {
        public string FileName { get; internal set; }
        public DateTime CreatedAt { get; set; }
    }
}
