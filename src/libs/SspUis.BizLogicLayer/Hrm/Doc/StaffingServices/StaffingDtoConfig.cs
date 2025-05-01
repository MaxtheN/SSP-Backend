using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;
using System;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    internal class StaffingDtoConfig : PerDtoConfig<StaffingDto, Staffing>
    {
        public override Action<IMappingExpression<Staffing, StaffingDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.FullName))
                .ForMember(x => x.Positions, x => x.MapFrom(ent => ent.Positions))
                .ForMember(x => x.SettlementAccountSource, x => x.MapFrom(ent => ent.SettlementAccountSource!=null? ent.SettlementAccountSource.Code:null))
                .ForMember(x => x.StaffingTemplateName, x => x.MapFrom(ent => ent.StaffingTemplate !=null? ent.StaffingTemplate.TemplateName:null))
                .ForMember(x => x.OrgSettlementAccountCode, x => x.MapFrom(ent => ent.OrgSettlementAccount != null ? ent.OrgSettlementAccount.AccountCode : null))
             ;
    }
}
