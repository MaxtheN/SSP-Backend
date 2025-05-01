using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public interface IBaseEntityService<TEntity, TListDto, TDto, TCreateDto, TUpdateDto>
        : IBaseEntityService<int, TEntity, TListDto, TDto, TCreateDto, TUpdateDto, SortFilterPageOptions>
        where TEntity : class, IHaveIdProp<int>
        where TListDto : class
        where TDto : class
        where TCreateDto : EntityDto<TCreateDto, TEntity>
        where TUpdateDto : EntityDto<TUpdateDto, TEntity>, IHaveIdProp<int>
    {
    }
}
