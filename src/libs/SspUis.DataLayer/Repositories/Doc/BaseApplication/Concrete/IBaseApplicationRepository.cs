using System;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public interface IBaseApplicationRepository
    <TId,
    TApplicationEntity,
    TCreateApplicationDto,
    TUpdateApplicationDto,
    TUpdateStatusDto>
    : IBaseEntityRepository<TId, TApplicationEntity, TCreateApplicationDto, TUpdateApplicationDto>
        where TApplicationEntity : class, IBaseApplicationEntity, IHaveIdProp<TId>
        where TCreateApplicationDto : EntityDto<TCreateApplicationDto, TApplicationEntity>
        where TUpdateApplicationDto : EntityDto<TUpdateApplicationDto, TApplicationEntity>, IHaveIdProp<TId>
{
    TApplicationEntity UpdateStatus(TUpdateStatusDto updateStatusDto, Action<TApplicationEntity> validation = null);
    TApplicationEntity UpdateStatus(TUpdateStatusDto updateStatusDto, Action<TApplicationEntity> validation, bool applyFilter);

}
