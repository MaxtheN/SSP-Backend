using AutoMapper;
using GenericServices.Configuration;
using SspUis.BizLogicLayer.Hrm.EmployeeManageServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;

namespace SspUis.BizLogicLayer.Notify
{
    public class SendSmsLogListDtoConfig : PerDtoConfig<SendSmsLogListDto, SendSmsLog>
    {
        public override Action<IMappingExpression<SendSmsLog, SendSmsLogListDto>> 
            AlterReadMapping => cfg => cfg
            .ForMember(dto => dto.Table, dto => dto.MapFrom(ent => ent.Table.FullName))
            .ForMember(dto => dto.FromStatus, dto => dto.MapFrom(ent => ent.Status.FullName))
            .ForMember(dto => dto.ToStatus, dto => dto.MapFrom(ent => ent.Status.FullName));
    }
}
