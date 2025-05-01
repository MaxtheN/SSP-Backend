using System;
using GenericServices.Configuration;
using AutoMapper;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.OrganizationalStructureServices
{
    public class OrganizationalStructureStaffingIndicatorTableDtoConfig : PerDtoConfig<OrganizationalStructureStaffingIndicatorTableDto, OrganizationalStructureStaffingIndicatorTable>
    {
        public override Action<IMappingExpression<OrganizationalStructureStaffingIndicatorTable, OrganizationalStructureStaffingIndicatorTableDto>> AlterReadMapping =>
            cfg => cfg.ForMember(x => x.StaffingIndicator, env => env.MapFrom(x => x.StaffingIndicator.ShortName));
    }
}
