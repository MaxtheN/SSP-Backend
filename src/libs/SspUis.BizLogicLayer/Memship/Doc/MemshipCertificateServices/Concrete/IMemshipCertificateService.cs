using SspUis.BizLogicLayer.Memship;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Memship;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public interface IMemshipCertificateService
    : IBaseEntityService<long, MemshipCertificate, MemshipCertificateListDto, MemshipCertificateDto, CreateMemshipCertificateDlDto, UpdateMemshipCertificateDlDto, MemshipCertificateSortFilterOptions>
{
    PagedResult<MemshipCertificateListDto> GetList(MemshipCertificateSortFilterOptions options);
    long GetCount();
    int GetCountMy();
	MemshipCertificateDto Get();
    MemshipCertificateDto Get(long id);
    MemshipCertificateDto GetByMemshipContractId(long memshipContractId);
    SelectList<long> AsSelectList();
    Task<HaveId<long>> Create(CreateMemshipCertificateDlDto dto);
    void Accept(UpdateStatusMemshipCertificateDto dTo);
    ValueTask Cancel(CancelStatusMemshipCertificateDto dto);
    void Update(UpdateMemshipCertificateDlDto dto);
    void Update(UpdatingBirdMemshipCertificateDlDto dto);
    void Delete(long id);
    Task SendToStat();
    object UploadFiles(StorageFile[] files);
    byte[] DownloadPdf(Guid id2, string? lang);
    StorageFile DownloadFile(Guid fileId);
    byte[] DownloadPdf(MemshipCertificateForPdf model);
    Task<string> ImportJismoniy(List<ImportJISMemshipCertificateDlDto> listDto);
    Stream SaveAsExcel(MemshipCertificateSortFilterOptions options);
    Task<string> ImportYuridik(List<ImportYurMemshipCertificateDlDto> listDto);
    byte[] DownloadPdf(string innPinfl, string? lang);
    ValueTask<MemshipCertificateFromSoliqDto> GetFromSoliq(string inn);
    void ProlongExpireOn(MemshipCertificateProlongDto dTo);
    void ProlongAosExpireOn();
    void ProlongQqsExpireOn();
    dynamic GetByInnPinflForChamber(string innPinfl);
    void ProlongExpireOnForPaid(long id, long paymentOrderId, string message);
    MemshipCertificateExpiredPaymentsDto MemshipCertificateExpiredPayment();
    MemshipCertificateToPaidNotificationsDto MemshipCertificateToPaidNotification();
}