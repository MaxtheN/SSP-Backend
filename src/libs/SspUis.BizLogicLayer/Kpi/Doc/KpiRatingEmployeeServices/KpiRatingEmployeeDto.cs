using System.Collections.Generic;
using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class KpiRatingEmployeeDto : UpdateKpiRatingEmployeeDlDto, ILinkToEntity<KpiRatingEmployee>, IHaveIdProp<long>, IDocument
{
    public string Status { get; set; }
    public string Organization { get; set; }

    public int StatusId { get; set; }
    public int TableId { get; set; }
    public int OrganizationId { get; set; }
    public List<KpiRatingEmployeeTableDto> Tables { get; set; } = new();

    #region Actions
    public bool CanModify { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion

}
