using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;

namespace SspUis.BizLogicLayer.Doc;

public class MonoApplicationResultConfig : PerDtoConfig<MonoApplicationBandlikResultDto, MonoApplicationBandlikResult>
{
    public override Action<IMappingExpression<MonoApplicationBandlikResult, MonoApplicationBandlikResultDto>> AlterReadMapping =>
       cfg => cfg
                .ForMember(x => x.ApplicationId, x => x.MapFrom(ent => ent.ApplicationId))
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status))
                .ForMember(x => x.SubsidyAmount, x => x.MapFrom(ent => ent.SubsidyAmount))
                .ForMember(x => x.ResponsibleFio, x => x.MapFrom(ent => ent.ResponsibleFio))
                .ForMember(x => x.ResponsiblePhone, x => x.MapFrom(ent => ent.ResponsiblePhone))
                .ForMember(x => x.RejectReason, x => x.MapFrom(ent => ent.RejectReason));
}