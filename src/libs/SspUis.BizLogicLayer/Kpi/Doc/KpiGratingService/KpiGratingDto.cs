using GenericServices;
using SspUis.BizLogicLayer.Kpi.Doc.KpiGratingService;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Kpi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class KpiGratingDto : UpdateKpiGratingDlDto, ILinkToEntity<KpiGrating>, IHaveIdProp<long>, IDocument
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

    public List<KpiGratingIndicatorDto> Indicators { get; set; } = new();
}
