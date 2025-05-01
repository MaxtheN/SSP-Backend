using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Interfaces;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Hrm.ContractorActivityTypeServices
{
    public class ContractorActivityTypeDto : UpdateContractorActivityTypeDlDto, ILinkToEntity<ContractorActivityType>, IInfoHl
    {
        public string State { get; set; } = null!;
        public string ContractorActivityGroup { get; set; }

        public new List<ContractorActivityTypeTranslateDto> Translates { get; set; } = new();
    }
}
