using System;
using AutoMapper;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Doc.BaseApplication
{
    public class UpdateStepApplicationDlDto<TApplicationStepDto, TApplicationEntity>
        : EntityDto<TApplicationStepDto, TApplicationEntity>, IHaveIdProp<long>
        where TApplicationStepDto : UpdateStepApplicationDlDto<TApplicationStepDto, TApplicationEntity>
        where TApplicationEntity : class, IBaseApplicationEntity
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }
        //public string? Message { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int CurrentStepId { get; set; }

        protected override Action<IMappingExpression<TApplicationStepDto, TApplicationEntity>> AlterMapping =>
            cfg => cfg
                .ForPath(x => x.Id, c => c.MapFrom(dto => dto.Id))
                //.ForPath(x => x.Application.Message, c => c.MapFrom(dto => dto.Message))
                .ForPath(x => x.Application.CurrentStepId, c => c.MapFrom(dto => dto.CurrentStepId));
    }
}
