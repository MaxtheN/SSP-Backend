using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Memship;

public class DebtDto : UpdateDebtDlDto, ILinkToEntity<Debt>
{

    public string Organization { get; set; }
    public string Status { get; set; }
    public decimal? TotalDebtAmount { get; set; }
    public decimal? TotalEntitlementAmount { get; set; }

    public new List<DebtTableDto> Tables { get; set; } = new();
}
