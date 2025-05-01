using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer
{
    public class RestrictionSendingAppDtoConfig : PerDtoConfig<RestrictionSendingAppDto, RestrictionOfSendingApplication>
    {
        public override Action<IMappingExpression<RestrictionOfSendingApplication, RestrictionSendingAppDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.Table, x => x.MapFrom(ent => ent.Table == null ? string.Empty : $"{ent.Table.DbSchemaName}.{ent.Table.FullName}"))
            .ForMember(x => x.ApplicationModelCode, x => x.MapFrom(ent => ent.ApplicationModelCode == null ? string.Empty : ent.ApplicationModelCode.FullName));
    }
}
