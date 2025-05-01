using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public interface IMassPlannedCalculationService
    : IBaseEntityService<long, MassPlannedCalculation, MassPlannedCalculationListDto, MassPlannedCalculationDto, CreateMassPlannedCalculationDlDto, UpdateMassPlannedCalculationDlDto,MassPlannedCalculationSortFilterOptions>
{
    PagedResult<MassPlannedCalculationListDto> GetList(MassPlannedCalculationSortFilterOptions options);
    MassPlannedCalculationDto Get();
    MassPlannedCalculationDto Get(long id);
    SelectList<long> AsSelectList();
    HaveId<long> Create(CreateMassPlannedCalculationDlDto dto);
    void Accept(UpdateStatusMassPlannedCalculationDto dTo);
    void Cancel(UpdateStatusMassPlannedCalculationDto dTo);
    void Update(UpdateMassPlannedCalculationDlDto dto);
    void Delete(long id);
}
