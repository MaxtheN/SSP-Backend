using SspUis.DataLayer.Repositories;
using StatusGeneric;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;
using System.Threading.Tasks;


namespace SspUis.BizLogicLayer.CustomJobServices;

public interface ICustomJobService : IStatusGeneric
{
    PagedResult<CustomJobListDto> GetList(CustomJobSortFilterOptions dto);
    CustomJobDto Get();
    CustomJobDto Get(long id);
    PagedResult<CustomJobListDto> GetTable(CustomJobSortFilerByIdOptions options);
    HaveId<long> Create(CreateCustomJobDlDto dto);
    HaveId<long> Update(UpdateCustomJobDlDto dto);
    Task<HaveId<long>> Approve(UpdateStatusCustomJobDto dto);
    Task<HaveId<long>> Cancel(long id);
    HaveId<long> UpdateStatus(UpdateStatusCustomJobDto dto, int statusId);
    void Delete(long id);
}