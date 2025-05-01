using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public class StaffingDlDto<TDto> : EntityDto<TDto, Staffing>
        where TDto : StaffingDlDto<TDto>
    {
        [LocalizedStringLength(250)]
        public string DocNumber { get; set; } = null!;
        [LocalizedRequired]
        public DateOnly DocOn { get; set; } = DateTime.Today.AsDateOnly();
        [LocalizedRequired]
        public DateOnly StartOn { get; set; } = DateTime.Today.AsDateOnly();
        [LocalizedRequired]
        public decimal DocSum { get; set; }
        public string? Details { get; set; } = null!;
        [LocalizedRequired]
        public int FinanceYear { get; set; } = DateTime.Today.Year;
        [LocalizedRequired]
        public int ForMonths { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int StaffingTypeId { get; set; }
        public int? OrganizationId { get; set; }
        public long? OrgSettlementAccountId { get; set; }
        //public long? StaffingTemplateId { get; set; } keremas dyishdi

        /// Shu 3 ta pasdagi column ba'zada majburiy emas hozircha dto da majburiy qilib quyildi. 
      /*  [LocalizedRequired]
        [LocalizedRange(1,int.MaxValue)]*/
        public int? SettlementAccountSourceId { get; set; }
        //[LocalizedRequired]
        //[LocalizedRange(1, int.MaxValue)]
        public int? LevelCodeId { get; set; }
        //[LocalizedRequired]
        //[LocalizedRange(1, int.MaxValue)]
        public int? SourceCodeId { get; set; }
        public List<StaffingIndicatorValueDlDto> IndicatorValues { get; set; } = new List<StaffingIndicatorValueDlDto>();
        public List<StaffingPositionDlDto> Positions { get; set; } = new List<StaffingPositionDlDto>();

        protected override Action<IMappingExpression<TDto, Staffing>> AlterMapping => cfg => cfg
           .ForMember(x => x.IndicatorValues, x => x.Ignore()).ForMember(x => x.Positions, x => x.Ignore());

        public override Staffing CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StatusId = StatusIdConst.CREATED;
            //entity.TableId = TableIdConst.SALARY__DOC_STAFFING;
            IndicatorValues.AddTo(entity.IndicatorValues);
            Positions.AddTo(entity.Positions, (e, d) =>
            {
                d.CalcKinds.AddTo(e.CalcKinds);
            });
            foreach (var positions in entity.Positions)
            {
                positions.ForMonth = 12;
                positions.PositionPeriodId = 1;

            }
            return entity;
        }

        public override void UpdateEntity(Staffing entity)
        {
            base.UpdateEntity(entity);
            IndicatorValues.ApplyChangesTo<long, StaffingIndicatorValueDlDto, StaffingIndicatorValue>(entity.IndicatorValues);
            Positions.ApplyChangesTo<long, StaffingPositionDlDto, StaffingPosition>(entity.Positions,
                (e, d) =>
                    d.CalcKinds.AddTo(e.CalcKinds),
                    (e, d) =>
                    {
                        d.CalcKinds.ApplyChangesTo<long, StaffingCalcKindDlDto, StaffingCalcKind>(e.CalcKinds);
                    }
                );
            entity.StatusId = StatusIdConst.MODIFIED;
        }
    }
}
