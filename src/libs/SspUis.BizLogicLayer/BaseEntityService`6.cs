using SspUis.DataLayer;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public abstract class BaseEntityService<TEntity, TListDto, TDto, TCreateDlDto, TUpdateDlDto, TRepository>
        : BaseEntityService<int, TEntity, TListDto, TDto, TCreateDlDto, TUpdateDlDto, TRepository, SortFilterPageOptions>
        where TEntity : class, IHaveIdProp<int>
        where TListDto : class
        where TDto : class, new()
        where TCreateDlDto : EntityDto<TCreateDlDto, TEntity>
        where TUpdateDlDto : EntityDto<TUpdateDlDto, TEntity>, IHaveIdProp<int>
        where TRepository : class, IBaseEntityRepository<int, TEntity, TCreateDlDto, TUpdateDlDto>
    {
        protected BaseEntityService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
