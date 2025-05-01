using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Corruption;
using SspUis.DataLayer.Interfaces;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Corruption.ContractorUnionActivityTypeServices
{
    public class ContractorUnionActivityTypeDto : UpdateContractorUnionActivityTypeDlDto, ILinkToEntity<ContractorUnionActivityType>, IInfoHl
    {
        public string State { get; set; } = null!;
        public new List<ContractorUnionActivityTypeTranslateDto> Translates { get; set; } = new();
    }
}
