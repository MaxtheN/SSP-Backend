using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Info.OrganizationalStructure
{
    public class OrganizationalStructureStaffingIndicatorDlDto : EntityDto<OrganizationalStructureStaffingIndicatorDlDto, OrganizationalStructureStaffingIndicator>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int OwnerId { get; set; }
        [LocalizedRequired]
        public bool IsTotal { get; set; }
        [LocalizedRequired]
        public bool IsCalculationKindTotal { get; set; }
        public int CalcOrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int DisplayOrderCode { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int StaffingIndicatorId { get; set; }
        [LocalizedRequired]
        public decimal Percentage { get; set; }
        public List<int> IndicatorTables { get; set; } = new List<int>();
        //public List<OrganizationalStructureStaffingIndicatorTableDlDto> Tables { get; set; } = new List<OrganizationalStructureStaffingIndicatorTableDlDto>();
        protected override Action<IMappingExpression<OrganizationalStructureStaffingIndicatorDlDto, OrganizationalStructureStaffingIndicator>> AlterMapping => cfg => cfg
            .ForMember(x => x.Tables, x => x.Ignore());

        public override OrganizationalStructureStaffingIndicator CreateEntity()
        {
            var entity = base.CreateEntity();
            //Tables.AddTo(entity.Tables); 
            entity.Tables.AddFromForeignKeys(IndicatorTables.Distinct());

            return entity;
        }

        public override void UpdateEntity(OrganizationalStructureStaffingIndicator entity)
        {
            base.UpdateEntity(entity);
            entity.Tables.UpdateFromForeignKeys(IndicatorTables.Distinct());
            //Tables.ApplyChangesTo<int, OrganizationalStructureStaffingIndicatorTableDlDto, OrganizationalStructureStaffingIndicatorTable>(entity.Tables);
        }
    }
}
