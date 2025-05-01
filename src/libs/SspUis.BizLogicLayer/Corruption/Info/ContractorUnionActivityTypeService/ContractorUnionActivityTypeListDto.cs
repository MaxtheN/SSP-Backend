using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Corruption;
using System;

namespace SspUis.BizLogicLayer.Corruption.ContractorUnionActivityTypeServices
{
    public class ContractorUnionActivityTypeListDto :  ILinkToEntity<ContractorUnionActivityType>
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string State { get; set; } = null!;
    }
}
