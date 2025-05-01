using Microsoft.AspNetCore.Http;
using StatusGeneric;
using System.Collections.Generic;
using System.IO;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.BizLogicLayer.Info.OrganizationalStructureServices;

namespace SspUis.BizLogicLayer.OrganizationalStructureServices
{
    public interface IOrganizationalStructureService : IBaseEntityService<OrganizationalStructure, OrganizationalStructureListDto, OrganizationalStructureDto, CreateOrganizationalStructureDlDto, UpdateOrganizationalStructureDlDto>, IStatusGeneric
    {
        decimal GetCorrCoef();
        SelectList<int> AsSelectList();
        PagedResult<OrganizationalStructureListDto> GetList(OrganizationalStructureFilterDto dto);
        List<OrganizationalStructureListDto> GetListDashboard(string parentCode, int? oblastId);
        List<OrganizationalStructureDashboardDto> GetListDashboard2();
        dynamic GetOrganizationCount();
        //Stream PrintOrganizationStructure(int id,bool isFullOrganization);
        //Stream PrintOrganizationalStructureList(OrganizationalStructureFilterDto dto);
        //Stream PrintOrganizationalStructureDashboard();
        void UploadEcxel(IFormFile file);
    }
}
