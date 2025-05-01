using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ExecutionApplicationDlDto<TDto> : EntityDto<TDto, ExecutionApplication>
        where TDto : ExecutionApplicationDlDto<TDto>
    {
        [LocalizedRequired]
        public DateOnly DocOn { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string DocNumber { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int Year { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int Month { get; set; }
        [LocalizedRequired]
        public int TotalNewVacanciesCount { get; set; }
        [LocalizedRequired]
        public decimal TotalPaymentAmount { get; set; }
        [LocalizedRequired]
        public decimal TotalAverageSalary { get; set; }
        public List<ExecutionApplicationTableDlDto> Tables { get; set; } = new();
        public List<ExecutionApplicationSignDlDto> Signers { get; set; } = new();
        protected override Action<IMappingExpression<TDto, ExecutionApplication>> AlterMapping =>
            cfg => cfg
             .ForMember(x => x.Tables, c => c.Ignore())
             .ForMember(x => x.Signs, c => c.Ignore());
        public override ExecutionApplication CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StatusId = StatusIdConst.CREATED;
            Tables.AddTo(entity.Tables);
            return entity;
        }
        public override void UpdateEntity(ExecutionApplication entity)
        {
            base.UpdateEntity(entity);
            entity.StatusId = StatusIdConst.MODIFIED;
            Tables.ApplyChangesTo<long, ExecutionApplicationTableDlDto, ExecutionApplicationTable>(entity.Tables);
        }
    }
}