using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Kpi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class KpiGratingListDto : DocumentListDto<long>, ILinkToEntity<KpiGrating>, IHaveIdProp<long>, IHaveStatusId
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
