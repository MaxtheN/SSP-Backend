using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class ServicePriceGroupListDto : ILinkToEntity<ServicePriceGroup>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public int? GroupId { get; set; }
        public string Group { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ServicePriceGroupTableListDto> Tables { get; set; }
    }
}
