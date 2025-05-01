using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;
using System;
using WEBASE.OfficeTools.Attributes;

namespace SspUis.BizLogicLayer
{
    public class SrvDeedGroupDto : ServiceDeedGroupDlDto,
        ILinkToEntity<ServiceDeedGroup>
    {
        public string Group { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [IgnoreWordProperty]
        public new List<SrvDeedTableDto> Tables { get; set; } = new();
    }
}
