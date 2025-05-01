using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Kpi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Kpi;

public class KpiPlanForEmployeeListDto : DocumentListDto<long>, ILinkToEntity<KpiPlanForEmployee>
{
    public string DocNumber { get; set; }
    public string Status { get; set; }
    public string Organization { get; set; }
    public int OrganizationId { get; set; }

    #region Actions
    public bool CanDelete { get; set; }
    public bool CanEdit { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    #endregion
}

