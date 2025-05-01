using AutoMapper;
using GenericServices.Configuration;
using SspUis.BizLogicLayer.NotificationServices;
using SspUis.DataLayer.EfClasses;
using System;

namespace SspUis.BizLogicLayer.Notify
{
    public class SendSmsConfigDtoConfig : PerDtoConfig<SendSmsConfigDto, SendSmsConfig>
    {
        public override Action<IMappingExpression<SendSmsConfig, SendSmsConfigDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Table, x => x.MapFrom(ent => ent.Table.FullName))
                .ForMember(x => x.FromStatus, x => x.MapFrom(ent => ent.FromStatus.FullName))
                .ForMember(x => x.ToStatus, x => x.MapFrom(ent => ent.ToStatus.FullName))
                .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.FullName));
    }
}
