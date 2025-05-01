using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Info.OrganizationalStructure;

namespace SspUis.BizLogicLayer.OrganizationalStructureServices
{
    public class OrganizationalStructureCalculationKindDto:OrganizationalStructureCalculationKindDlDto,ILinkToEntity<OrganizationalStructureCalculationKind>
    {
        public string CalculationKind { get; set;}
    }
    public class OrganizationalStructureCalculationKindConfigDto:PerDtoConfig<OrganizationalStructureCalculationKindDto,OrganizationalStructureCalculationKind>
    {
        public override Action<IMappingExpression<OrganizationalStructureCalculationKind, OrganizationalStructureCalculationKindDto>> AlterReadMapping => 
            cfg => cfg.ForMember(x=>x.CalculationKind,env=> env.MapFrom(x=> x.CalculationKind.ShortName));
    }
}
