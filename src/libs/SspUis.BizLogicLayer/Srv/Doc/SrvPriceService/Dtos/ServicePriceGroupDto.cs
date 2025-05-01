using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer
{
    public class ServicePriceGroupDto : ServicePriceGroupDlDto,
        ILinkToEntity<ServicePriceGroup>
    {
        new public List<ServicePriceGroupTableDto> Tables { get; set; } = new();
        public int? GroupId { get; set; }
        public string Group { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}