using System;
using System.Linq;

using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;

using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.ExecutionApplicationServices;

public class ExecutionApplicationTableDtoConfig : PerDtoConfig<ExecutionApplicationTableDto, ExecutionApplicationTable>
{
    public override Action<IMappingExpression<ExecutionApplicationTable, ExecutionApplicationTableDto>> AlterReadMapping =>
        cfg => cfg
                .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName)) 
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn)) 
           ;
}
