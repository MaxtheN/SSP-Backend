using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Doc.BaseApplication
{
    public class ApplicationDlDto : EntityDto<ApplicationDlDto, Application>
    {
        [LocalizedRequired]
        public DateOnly DocOn { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string DocNumber { get; set; }
        //[LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ContractorPositionName { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int ApplicationTypeId { get; set; }
        public long? ContractorSettlementAccountId { get; set; }
        [IgnoreMap]
        public long? ContractorId { get; set; }
        public List<ApplicationStepDlDto> Steps { get; set; } = new();

        protected override Action<IMappingExpression<ApplicationDlDto, Application>> AlterMapping =>
            cfg => cfg.ForMember(x => x.Steps, c => c.Ignore());
        public override Application CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.Id2 = Guid.NewGuid();
            entity.StatusId = StatusIdConst.CREATED;
            entity.TableId = TableIdConst.DOC_APPLICATION;
            Steps.AddTo(entity.Steps);
            return entity;
        }

        public override void UpdateEntity(Application entity)
        {
            Steps.ApplyChangesTo<long, ApplicationStepDlDto, ApplicationStep>(entity.Steps);
            base.UpdateEntity(entity);
            entity.StatusId = StatusIdConst.MODIFIED;
        }
    }
}
