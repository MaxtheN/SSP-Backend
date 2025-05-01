using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;

namespace SspUis.BizLogicLayer.Hrm.ContractorActivityGroupServices
{
    public class ContractorActivityGroupListDto :  ILinkToEntity<ContractorActivityGroup>
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string State { get; set; } = null!;
    }
}
