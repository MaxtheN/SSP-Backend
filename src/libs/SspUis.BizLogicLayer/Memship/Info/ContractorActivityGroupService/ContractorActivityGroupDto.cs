using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Interfaces;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Hrm.ContractorActivityGroupServices
{
    public class ContractorActivityGroupDto : UpdateContractorActivityGroupDlDto, ILinkToEntity<ContractorActivityGroup>, IInfoHl
    {
        public string State { get; set; } = null!;
        public new List<ContractorActivityGroupTranslateDto> Translates { get; set; } = new();
    }
}
