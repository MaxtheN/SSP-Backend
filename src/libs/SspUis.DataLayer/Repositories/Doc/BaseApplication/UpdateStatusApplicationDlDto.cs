using System;
using AutoMapper;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Doc.BaseApplication
{
    public class UpdateStatusApplicationDlDto<TApplicationStatusDto, TApplicationEntity> : EntityDto<TApplicationStatusDto, TApplicationEntity>, IHaveIdProp<long>
        where TApplicationStatusDto : UpdateStatusApplicationDlDto<TApplicationStatusDto, TApplicationEntity>
        where TApplicationEntity : class, IBaseApplicationEntity
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }
        public string? Message { get; set; }
        public bool IsRead { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int StatusId { get; set; }

        protected override Action<IMappingExpression<TApplicationStatusDto, TApplicationEntity>> AlterMapping =>
            cfg => cfg
                .ForPath(x => x.Id, c => c.MapFrom(dto => dto.Id))
                .ForPath(x => x.Application.Message, c => c.MapFrom(dto => dto.Message))
                .ForPath(x => x.Application.StatusId, c => c.MapFrom(dto => dto.StatusId));
    }
}
