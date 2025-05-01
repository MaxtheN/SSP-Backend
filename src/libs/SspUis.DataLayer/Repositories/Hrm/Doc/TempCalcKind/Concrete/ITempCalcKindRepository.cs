using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public interface ITempCalcKindRepository
    :IBaseEntityRepository<long,TempCalcKind,CreateTempCalcKindDlDto,UpdateTempCalcKindDlDto,UpdateStatusTempCalcKindDlDto>
{
}
