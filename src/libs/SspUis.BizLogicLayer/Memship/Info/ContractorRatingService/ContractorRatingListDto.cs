using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Memship;

public class ContractorRatingListDto : ILinkToEntity<ContractorRating>
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string State { get; set; } = null!;
    public string ContractorType { get; set; }
    public decimal MinimumPercentage { get; set; }

    public decimal MaximumPercentage { get; set; }
    public int Score { get; set; }
}
