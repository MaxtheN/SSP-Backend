using System;
using SspUis.DataLayer.Repositories.Doc.BaseApplication;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;
public interface IBaseApplicationRepository
    <TId,
    TApplicationEntity,
    TCreateApplicationDto,
    TUpdateApplicationDto,
    TUpdateStatusDto,
    TUpdateStepDto>
    : IBaseEntityRepository<TId, TApplicationEntity, TCreateApplicationDto, TUpdateApplicationDto>
        where TApplicationEntity : class, IBaseApplicationEntity, IHaveIdProp<TId>
        where TCreateApplicationDto : EntityDto<TCreateApplicationDto, TApplicationEntity>
        where TUpdateApplicationDto : EntityDto<TUpdateApplicationDto, TApplicationEntity>, IHaveIdProp<TId>
        where TUpdateStepDto : UpdateStepApplicationDlDto<TUpdateStepDto, TApplicationEntity>
{
    TApplicationEntity UpdateStatus(TUpdateStatusDto updateStatusDto, Action<TApplicationEntity> validation = null);
    TApplicationEntity UpdateStatus(TUpdateStatusDto updateStatusDto, Action<TApplicationEntity> validation, bool applyFilter);
    TApplicationEntity UpdateStep(TUpdateStepDto updateStepDto, Action<TApplicationEntity> validation = null)
        //where TUpdateStepDto : UpdateStepApplicationDlDto<TUpdateStepDto, TApplicationEntity>
        ;
    TApplicationEntity UpdateStep(TUpdateStepDto updateStepDto, Action<TApplicationEntity> validation, bool applyFilter)
        //where TUpdateStepDto : UpdateStepApplicationDlDto<TUpdateStepDto, TApplicationEntity>
        ;

}
