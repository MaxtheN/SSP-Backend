using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.EfCode;
using SspUis.DataLayer.Repositories.Hrm;
using System;

using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Kpi;

public interface IKpiGratingRepository:IBaseEntityRepository<long,KpiGrating, CreateKpiGratingDlDto, UpdateKpiGratingDlDto, UpdateStatusKpiGratingDlDto>

{
    
}
