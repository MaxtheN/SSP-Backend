using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.MediationServices;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Claim;
using StatusGeneric;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Claim;

public interface IMediationService : IStatusGeneric
{
    PagedResult<MediationListDto> GetList(MediationSortFilterOptions options);
    MediationDto Get();
    MediationDto Get(long id);
    HaveId<long> Create(CreateMediationDlDto dto);
    void Update(UpdateMediationDlDto dto);
    void Delete(long id);
    IEnumerable<MediationFileDto> UploadFiles(params StorageFile[] files);
    StorageFile DownloadFile(Guid fileId);
    void DeleteFile(Guid fileId);
    MediationDto GetByPlanId(int planId);
    byte[] DownloadPdf(Guid id2, string? lang);
    HaveId<long> Accept(CreateMediationPlanIFMeditionReviewDlDto dto);
    HaveId<long> Cancel(long id, string? message);
}
