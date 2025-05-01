using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.OrganizationServices
{
    public interface IOrganizationService : IStatusGeneric
    {
        PagedResult<OrganizationListDto> GetList(OrganizationSortFilterPageOptions dto);
        OrganizationDto Get();
        Task<OrganizationDto> GetByInn(string inn);
        OrganizationDto Get(int id);
        SelectList<int> AsSelectList(int? parentId = null, bool authorizedOnly = false, bool inspectionOnly = false, int? signOrganizationTypeId = null, int? organizationGroupId = null);
        SelectList<long> AsSelectListOrgSettlementAccount();
        SelectList<int> AsSelectListOrgForBank(int? langId);
        Task<HaveId<int>> Create(CreateOrganizationDlDto dto);
        Stream SaveAsExecel(OrganizationSortFilterPageOptions dto);
        Task Update(UpdateOrganizationDlDto dto);
        void Delete(int id);
        string GetOrganizationNameByLocation(int regionId, int districtId, int prtnContractTypeId);
        Organization GetOrganizationByLocation(int regionId, int districtId, int prtnContractTypeId);
        Task<string> CheckPersonFromGsp(List<CheckPersonFromGspDlDto> listDto);
        void UpdateStructure(int organizationId, int? structureId);
        IEnumerable<OrganizationFileDto> UploadFiles(params StorageFile[] files);
        StorageFile DownloadFile(Guid fileId);
        List<OrganizationMyPageDto> GetListForMyPage();
        void DeleteFile(Guid fileId);
        SelectList<int> SelectListByRegionInOrganizations(int? regionId);
    }
}
