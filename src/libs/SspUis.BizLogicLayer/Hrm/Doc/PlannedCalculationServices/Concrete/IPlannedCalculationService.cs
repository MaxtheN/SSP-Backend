using System;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public interface IPlannedCalculationService
    : IBaseEntityService<long, PlannedCalculation, PlannedCalculationListDto, PlannedCalculationDto, CreatePlannedCalculationDlDto, UpdatePlannedCalculationDlDto,PlannedCalculationSortFilterOptions>
{
    PagedResult<PlannedCalculationListDto> GetList(PlannedCalculationSortFilterOptions options);
    PlannedCalculationDto Get();
    SelectList<long> AsSelectList(PlannedCalculationSortFilterOptions options);
    HaveId<long> Create(CreatePlannedCalculationDlDto dto);
    void Update(UpdatePlannedCalculationDlDto dto);
    void Delete(long id);
    Task<byte[]> DownloadPdf(Guid id2);
}
