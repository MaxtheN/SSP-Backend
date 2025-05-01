using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.CalculationKindServices
{
    public class CalculationKindStructureDtoConfig:PerDtoConfig<CalculationKindStructureDto, CalculationKindStructure>
    {
        public override Action<IMappingExpression<CalculationKindStructure, CalculationKindStructureDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.OrganizationalStructure, x => x.MapFrom(ent => ent.OrganizationStructure.Code));

    }
}
        

    
