using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer
{
    public class SrvApplicationTableDto : ServiceApplicationTableDlDto,
        ILinkToEntity<ServiceApplicationTable>
    {
        public string NeedChamberService { get; set; }
        public bool? IsCompleted { get; set; }
        public new List<SrvApplicationTableFileDto> Files { get; set; } = new();
    }
}