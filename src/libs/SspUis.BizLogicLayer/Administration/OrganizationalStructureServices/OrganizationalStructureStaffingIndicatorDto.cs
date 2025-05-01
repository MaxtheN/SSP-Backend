using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Info.OrganizationalStructure;

namespace SspUis.BizLogicLayer.OrganizationalStructureServices
{
    public class OrganizationalStructureStaffingIndicatorDto : OrganizationalStructureStaffingIndicatorDlDto, ILinkToEntity<OrganizationalStructureStaffingIndicator>
    {
        public string StaffingIndicator{ get; set; }
        public List<int> IndicatorTables { get; set; } = new();
    }
    public class OrganizationalStructureStaffingIndicatorConfigDto : PerDtoConfig<OrganizationalStructureStaffingIndicatorDto, OrganizationalStructureStaffingIndicator>
    {
        public override Action<IMappingExpression<OrganizationalStructureStaffingIndicator, OrganizationalStructureStaffingIndicatorDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.StaffingIndicator, env => env.MapFrom(x => x.StaffingIndicator.ShortName))
                .ForMember(x => x.IndicatorTables, env => env.MapFrom(x => x.Tables.Select(a => a.StaffingIndicatorId)))
            ;
    }
}
