using SspUis.DataLayer;
using SspUis.DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
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
    public abstract class BaseEntityService<TId, TEntity, TListDto, TDto, TCreateDto, TCreateDlDto, TUpdateDto, TUpdateDlDto, TRepository, TSortFilterPageOptions>
        : StatusGenericHandler, IBaseEntityService<TId, TEntity, TListDto, TDto, TCreateDto, TCreateDlDto, TUpdateDto, TUpdateDlDto, TSortFilterPageOptions>
        where TId : struct
        where TEntity : class, IHaveIdProp<TId>
        where TListDto : class
        where TDto : class, new()
        where TCreateDto : EntityDto<TCreateDlDto, TEntity>, TCreateDlDto
        where TUpdateDto : EntityDto<TUpdateDlDto, TEntity>, IHaveIdProp<TId>, TUpdateDlDto
        where TCreateDlDto : EntityDto<TCreateDlDto, TEntity>
        where TUpdateDlDto : EntityDto<TUpdateDlDto, TEntity>, IHaveIdProp<TId>
        where TRepository : class, IBaseEntityRepository<TId, TEntity, TCreateDlDto, TUpdateDlDto>
        where TSortFilterPageOptions : class, IPageOptions
    {
        public BaseEntityService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            Repository = unitOfWork.GetRepository<TRepository>();
        }

        protected IUnitOfWork UnitOfWork { get; }
        protected TRepository Repository { get; }

        protected virtual IQueryable<TListDto> SortFilter(IQueryable<TListDto> query, TSortFilterPageOptions options)
        {
            return query;
        }

        public virtual PagedResult<TListDto> GetList(TSortFilterPageOptions options)
        {
            var result = SortFilter(Repository.ReadAsNoTracked<TListDto>(), options)
                                .AsPagedResult(options);
            return result;
        }

        public virtual TDto Get()
        {
            return new TDto();
        }

        public virtual TDto Get(TId id)
        {
            var dto = Repository.ById<TDto>(id);
            CombineStatuses(Repository);
            return dto;
        }

        public virtual HaveId<TId> Create(TCreateDto dto)
        {
            var entity = Repository.Create(dto);
            CombineStatuses(Repository);
            if (IsValid)
            {
                UnitOfWork.Save();
                return HaveId.Create(entity.Id);
            }
            return null;
        }

        public virtual void Update(TUpdateDto dto)
        {
            Repository.Update(dto);
            CombineStatuses(Repository);
            if (IsValid)
                UnitOfWork.Save();
        }

        public virtual void Delete(TId id)
        {
            try
            {
                Repository.Delete(id);
                CombineStatuses(Repository);
                if (IsValid)
                    UnitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                AddError("Запись не может быть удален");
            }
        }
    }
}
