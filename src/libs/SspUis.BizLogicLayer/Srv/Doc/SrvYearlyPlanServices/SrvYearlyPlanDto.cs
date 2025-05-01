using System.Collections.Generic;
using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class SrvYearlyPlanDto : UpdateSrvYearlyPlanDlDto, ILinkToEntity<SrvYearlyPlan>, IHaveIdProp<long>, IDocument
{
    public string Status { get; set; }
    public string Organization { get; set; }

    public int StatusId { get; set; }
    public int TableId { get; set; }
    public int OrganizationId { get; set; }
    public List<SrvYearlyPlanTableCellDto> CellTables { get; set; } = new();
    public List<SrvYearlyPlanFileDto> Files { get; set; } = new();
    [JsonIgnore]
    public List<SrvYearlyPlanTableRegionDto> Regions { get; set; } = new();

    #region Actions
    public bool CanModify { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion

}
