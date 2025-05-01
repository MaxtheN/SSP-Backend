using GenericServices;
using SspUis.DataLayer.EfClasses;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class NeedChamberServiceGroupingDto
        : ILinkToEntity<NeedChamberService>,
        IHaveIdProp<int>
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public string Group { get; set; }
        public List<NeedChamberServiceGroupingTableListDto> Tables { get; set; }
    }
    public class NeedChamberServiceGroupingTableListDto
    {
        public long Id { get; set; }
        public int NeedChamberServiceId { get; set; }
        public string NeedChamberService { get; set; }
        public int ServicePriceTypeId { get; set; }
        public string ServicePriceType { get; set; }
    }
}