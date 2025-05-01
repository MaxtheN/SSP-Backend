using AutoMapper;
using WEBASE.Attributes;
using WEBASE.EF;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using SspUis.Core;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public class StaffingTemplateDlDto<TDto> : EntityDto<TDto, StaffingTemplate>
        where TDto : StaffingTemplateDlDto<TDto>
    {
        [LocalizedStringLength(30)]
        public string DocNumber { get; set; } = null!;
        [LocalizedRequired]
        public DateOnly DocOn { get; set; } = DateTime.Today.AsDateOnly();
        public string? Details { get; set; }
        [LocalizedRequired]
        public string TemplateName { get; set; } = null!;
        public int? OrganizationId { get; set; }

        public List<StaffingTemplateTableDlDto> Tables { get; set; } = new List<StaffingTemplateTableDlDto>();

        protected override Action<IMappingExpression<TDto, StaffingTemplate>> AlterMapping => cfg => cfg
            .ForMember(x => x.Tables, x => x.Ignore());

        public override StaffingTemplate CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StatusId = StatusIdConst.CREATED;
            Tables.AddTo(entity.Tables);
            return entity;
        }

        public override void UpdateEntity(StaffingTemplate entity)
        {
            base.UpdateEntity(entity);
            entity.StatusId = StatusIdConst.MODIFIED;
            Tables.ApplyChangesTo<long, StaffingTemplateTableDlDto, StaffingTemplateTable>(entity.Tables);
        }
    }
}
