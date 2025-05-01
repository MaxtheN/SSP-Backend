using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.Repositories.Kpi;
using System;
using System.Collections.Generic;

using WEBASE.Models;

namespace SspUis.BizLogicLayer.Kpi;

public class KpiPlanForEmployeeDto : UpdateKpiPlanForEmployeeDlDto, ILinkToEntity<KpiPlanForEmployee>, IHaveIdProp<long>,IDocument
{
    public string Status { get; set; }
    public string Organization { get; set; }
    public int OrganizationId { get; set; }
    public int StatusId { get; set; }

    #region Actions
    public bool CanModify { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion

    public List<KpiPlanForEmployeeTableDto> Tables { get; set; } = new();
}
