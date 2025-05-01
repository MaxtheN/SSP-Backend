using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;
using System;

namespace SspUis.BizLogicLayer
{
    public class SrvApplicationGroupDto : ServiceApplicationGroupDlDto,
        ILinkToEntity<ServiceApplicationGroup>
    {
        public new List<SrvApplicationTableDto> Tables { get; set; } = new();
        public string Group { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}