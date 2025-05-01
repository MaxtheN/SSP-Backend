using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Kpi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public interface IKpiGratingService : IBaseEntityService<long, KpiGrating, KpiGratingListDto, KpiGratingDto, CreateKpiGratingDlDto, UpdateKpiGratingDlDto, KpiGratingSortFilterOptions>

{
    PagedResult<KpiGratingListDto> GetList(KpiGratingSortFilterOptions options);
    KpiGratingDto Get();
    List<KpiGratingIndicatorDto> FillIndicator();
    KpiGratingDto Get(long id);
    SelectList<long> AsSelectList();
    HaveId<long> Create(CreateKpiGratingDlDto dto);
    void Accept(UpdateStatusKpiGratingDlDto dTo);
    void Cancel(UpdateStatusKpiGratingDlDto dTo);
    void Update(UpdateKpiGratingDlDto dto);
    void Delete(long id);
}
