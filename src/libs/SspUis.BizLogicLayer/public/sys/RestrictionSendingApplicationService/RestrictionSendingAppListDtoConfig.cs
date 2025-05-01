using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;

namespace SspUis.BizLogicLayer
{
    public class RestrictionSendingAppListDtoConfig : PerDtoConfig<RestrictionSendingAppListDto, RestrictionOfSendingApplication>
    {
        public override Action<IMappingExpression<RestrictionOfSendingApplication, RestrictionSendingAppListDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.Table, x => x.MapFrom(ent => ent.Table == null ? string.Empty : $"{ent.Table.DbSchemaName}.{ent.Table.FullName}"))
            .ForMember(x => x.ApplicatonModel, x => x.MapFrom(ent => ent.ApplicationModelCode == null ? string.Empty : ent.ApplicationModelCode.FullName))
            .ForMember(x => x.State, x => x.MapFrom(ent => ent.State == null ? string.Empty : ent.State.FullName));
    }
}
