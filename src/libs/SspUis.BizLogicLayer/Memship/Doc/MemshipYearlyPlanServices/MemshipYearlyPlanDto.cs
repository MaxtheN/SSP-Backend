using System.Collections.Generic;
using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Memship;
using SspUis.DataLayer.Repositories.Memship;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipYearlyPlanDto : UpdateMemshipYearlyPlanDlDto, ILinkToEntity<MemshipYearlyPlan>, IHaveIdProp<long>, IDocument
{
    public string Status { get; set; }
    public string Organization { get; set; }

    public int StatusId { get; set; }
    public int TableId { get; set; }
    public int OrganizationId { get; set; }
    public List<MemshipYearlyPlanTableCellDto> CellTables { get; set; } = new();
    public List<MemshipYearlyPlanFileDto> Files { get; set; } = new();
    [JsonIgnore]
    public List<MemshipYearlyPlanTableDto> Tables { get; set; } = new();

    #region Actions
    public bool CanModify { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion

}
