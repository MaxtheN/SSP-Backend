using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Minio.DataModel;
using SspUis.BizLogicLayer.Claim.Doc.ApplicationForCourtServices;
using SspUis.BizLogicLayer.Srv;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Sud.Models;
using SspUis.Integration.Sud.Models.AuthModels;
using SspUis.Integration.Sud.Models.MalumotnomaModel;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Claim;

public interface IApplicationForCourtService
    : IBaseEntityService<long
        , ApplicationForCourt
        , ApplicationForCourtListDto
        , ApplicationForCourtDto
        , CreateApplicationForCourtDlDto
        , UpdateApplicationForCourtDlDto
        , ApplicationForCourtSortFilterOptions>
{
    ValueTask<string> PostToIMZOAndSentUrl(ApplicationForCourt contract);
    ValueTask<(string? Url, bool Result)> WebImzoSign(WebImzoSignedFilter filter);
    Task Accept(AcceptUpdateStatusApplicationForCourtDlDto dto);
    Task<EImzoVerifyResultDto> AcceptCourt(AcceptUpdateStatusApplicationForCourtDlDto dto);
    Task AcceptForEmployee(AcceptUpdateStatusApplicationForCourtDlDto dto);
    Task<CheckAmountFromBankDto> ChekAllBanks(long mediationId);
    Task<CheckAmountFromBankDto> ChekFromAgroBank(long mediationId);
    Task<CheckAmountFromBankDto> ChekFromXalqBank(long mediationId);
    (StorageFile, int?, int) DownloadFile(Guid fileId, bool isGenerationQrCode);
    ValueTask<byte[]> DownloadFileWithQrCode(Guid id);
    ValueTask<byte[]> DownloadPdf(Guid id2);
    ApplicationForCourtDto GetByMediationId(long mediationId);
    void Reject(long id, string message);
    void Send(long id);
    IEnumerable<IStorageFileInfo> UploadFiles(params StorageFile[] files);
    ApplicationForCourtForGetDto GetForFiles(long mediationId);
    Guid SaveFile(long docId, string data, string fileName);
    HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null);
    Task<byte[]> DownloadBankDocument(long mediationId);
    Task<byte[]> DownloadXalqBankDocument(long mediationId, string? lang);
    //sud
    Task<UploadFileResponseDto> SudUploadFileIntegration(IFormFile file);
    Task<object> SendSudIntegration(CreateCourtntegrationDlDto dto);
    Task<InvoiceResponseModel> SudInvoice(SudInvoiceDto dto);
    Task<List<CommonEntityRegion>> GetSudRegionList(); 
    Task<List<CommonEntityRegion>> GetSudDistrictList(Guid regionId); 
    Task<List<CommonEntity>> GetSudPostReasonList();
    Task<List<string>> GetSudParticipantTypeList();
    Task<List<CommonEntity>> GetSudDutyReasonList(); 
    Task<List<CommonEntity>> GetSudDocumentTypesList();
    Task<List<string>> GetSudEntityTypeList(); 
    Task<List<CurrencyModel>> GetSudCurrencyList();
    Task<List<CommonEntity>> GetSudCourtList();
    Task<List<CommonEntity>> GetSudCountryList(); 
    Task<List<string>> GetSudClaimKindList();
    Task<List<CommonEntity>> GetSudCategoriesSubList();
    Task<List<CommonEntity>> GetSudCategoriesSecondList();
    Task<List<CommonEntity>> GetSudAmountCategoryList();
    Task<List<BankModel>> GetSudBankList();
    Task<List<CommonEntity>> GetSudCategoryList();

}
