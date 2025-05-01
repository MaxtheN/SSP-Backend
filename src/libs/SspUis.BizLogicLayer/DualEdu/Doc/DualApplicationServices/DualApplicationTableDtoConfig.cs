using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.Integration.Billing.Services;

namespace SspUis.BizLogicLayer.DualApplicationServices;

public class DualApplicationTableDtoConfig : PerDtoConfig<DualApplicationTableDto, DualApplicationTable>
{
    public override Action<IMappingExpression<DualApplicationTable, DualApplicationTableDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.Institute, x => x.MapFrom(ent => ent.Institute.FullName))
            .ForMember(x => x.Specialty, x => x.MapFrom(ent => ent.Specialty.FullName))
            .ForMember(x => x.PositionClassification, x => x.MapFrom(ent => ent.PositionClassification.FullName));
}