using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;

namespace SspUis.BizLogicLayer.DualEdu;

public class DualContractSignDtoConfig : PerDtoConfig<DualContractSignDto, DualContractSign>
{
    public override Action<IMappingExpression<DualContractSign, DualContractSignDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.DocDate, c => c.MapFrom(e => e.Owner.DocDate))
            .ForMember(d => d.DocNumber, c => c.MapFrom(e => e.Owner.DocNumber));
}