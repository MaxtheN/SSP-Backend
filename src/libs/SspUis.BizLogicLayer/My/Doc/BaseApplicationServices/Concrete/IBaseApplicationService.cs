using System.Threading.Tasks;
using StatusGeneric;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public interface IBaseApplicationService<TId, TEntity, TListDto, TDto, TCreateDto, TUpdateDto, TSortFilterPageOptions>
    : IStatusGeneric
    where TId : struct
    where TEntity : class, IHaveIdProp<TId>
    where TListDto : class
    where TDto : class
    where TCreateDto : EntityDto<TCreateDto, TEntity>
    where TUpdateDto : EntityDto<TUpdateDto, TEntity>, IHaveIdProp<TId>
    where TSortFilterPageOptions : class, IPageOptions
{
    PagedResult<TListDto> GetList(TSortFilterPageOptions dto);
    TDto Get();
    TDto Get(TId id);
    HaveId<TId> Create(TCreateDto dto);
    void Update(TUpdateDto dto);
    void Delete(TId id);
}
