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
    public interface IBaseEntityService<TId, TEntity, TListDto, TDto, TCreateDto, TUpdateDto>
        : IBaseEntityService<TId, TEntity, TListDto, TDto, TCreateDto, TUpdateDto, SortFilterPageOptions>
        where TId : struct
        where TEntity : class, IHaveIdProp<TId>
        where TListDto : class
        where TDto : class
        where TCreateDto : EntityDto<TCreateDto, TEntity>
        where TUpdateDto : EntityDto<TUpdateDto, TEntity>, IHaveIdProp<TId>
    {
    }

    //public interface ISimpleBaseEntityService<TEntity, TListDto, TDto, TCreateDto, TUpdateDto, TSortFilterPageOptions>
    //    : IBaseEntityService<int, TEntity, TListDto, TDto, TCreateDto, TUpdateDto, TSortFilterPageOptions>
    //    where TEntity : class, IHaveIdProp<int>
    //    where TListDto : class
    //    where TDto : class
    //    where TCreateDto : EntityDto<TCreateDto, TEntity>
    //    where TUpdateDto : EntityDto<TUpdateDto, TEntity>, IHaveIdProp<int>
    //    where TSortFilterPageOptions : class, IPageOptions
    //{
    //}
}
