using System;
using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Info.OrganizationalStructure;

namespace SspUis.BizLogicLayer.OrganizationalStructureServices
{
    public class OrganizationalStructureStaffingIndicatorTableDto : OrganizationalStructureStaffingIndicatorTableDlDto, ILinkToEntity<OrganizationalStructureStaffingIndicatorTable>
    {
        public string StaffingIndicator { get; set; }
    }
}