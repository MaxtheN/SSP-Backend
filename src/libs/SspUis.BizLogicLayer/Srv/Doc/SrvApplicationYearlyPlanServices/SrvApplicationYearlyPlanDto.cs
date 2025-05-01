using System.Collections.Generic;
using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class SrvApplicationYearlyPlanDto : UpdateSrvApplicationYearlyPlanDlDto, ILinkToEntity<SrvApplicationYearlyPlan>, IHaveIdProp<long>, IDocument
{
    public string Status { get; set; }
    public string Organization { get; set; }
    public string Region { get; set; }

    public int StatusId { get; set; }
    public int TableId { get; set; }
    public int OrganizationId { get; set; }
    public List<SrvApplicationYearlyPlanTableCellDto> CellTables { get; set; } = new();
    public List<SrvApplicationYearlyPlanFileDto> Files { get; set; } = new();
    [JsonIgnore]
    public List<SrvApplicationYearlyPlanTableDto> Tables { get; set; } = new();

    #region Actions
    public bool CanModify { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion

}
