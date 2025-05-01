using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;
using System;

namespace SspUis.BizLogicLayer
{
    public class SrvContractGroupDto : ServiceContractGroupDlDto,
        ILinkToEntity<ServiceContractGroup>
    {
        public string Group { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public new List<SrvContractTableDto> Tables { get; set; } = new();
    }
}
