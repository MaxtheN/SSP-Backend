

using GenericServices;
using SspUis.BizLogicLayer.Hrm.ContractorActivityTypeServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Interfaces;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SspUis.BizLogicLayer.Memship;

public class ContractorRatingDto : UpdateContractorRatingDlDto, ILinkToEntity<ContractorRating>, IInfoHl
{
    public string State { get; set; } = null!;
    public string ContractorType { get; set; }
    public decimal MinimumPercentage { get; set; }
    public decimal MaximumPercentage { get; set; }
    public int Score { get; set; }
   
    public new List<ContractorRatingTranslateDto> Translates { get; set; } = new();
}
