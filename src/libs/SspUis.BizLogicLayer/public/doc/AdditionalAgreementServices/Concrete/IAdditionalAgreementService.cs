using System;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.AdditionalAgreementService;

public interface IAdditionalAgreementService : IBaseEntityService<long, AdditionalAgreement, AdditionalAgreementListDto, AdditionalAgreementDto, CreateAdditionalAgreementDlDto, UpdateAdditionalAgreementDlDto>
{
    PagedResult<AdditionalAgreementListDto> GetList(AdditionalAgreementSortFilterOption dto);
    //AdditionalAgreementDto Get();
    //AdditionalAgreementDto Get(long id);
    SelectList<long> AsSelectList();
    Task<byte[]> DownloadPdf(Guid id2, string? lang);
    MemshipAdditionalAgreementDto GetByMemshipContractId(long memshipContractId);
    Task Reject(RejectStatusAdditionalAgreementDto dto);
    Task Sign(SignStatusAdditionalAgreementDto dto);
    //HaveId<long> Create(CreateAdditionalAgreementDlDto dto);
    //void Update(UpdateAdditionalAgreementDlDto dto);
    //void Delete(long id);
}
