using SspUis.BizLogicLayer.Srv.Doc.SrvApplicationService.Dtos;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer
{
    public interface ISrvApplicationService
        : IBaseApplicationService
            <long,
            ServiceApplication,
            SrvApplicationListDto,
            SrvApplicationDto,
            CreateServiceApplicationDlDto,
            UpdateServiceApplicationDlDto,
            SrvApplicationSortFilterOptions>
    {
        SelectList<long> AsSelectList(SrvApplicationSortFilterOptions options);
        ValueTask<HaveId<long>> CreateSrv(CreateServiceApplicationDlDto dto);
        void Accept(AcceptStatusSrvApplicationDto dto);
        void AcceptForFree(AcceptStatusSrvApplicationDto dto);
        void Cancel(CancelStatusSrvApplicationDto dto);
        void Reject(RejectStatusSrvApplicationDto dto);
        Task SendAsync(SendStatusSrvApplicationDto dto, bool isCreate);
        SrvApplicationDto Get(Guid id2);
        int GetCount();
        int GetPayedCount();
        int GetFreeCount();
        IEnumerable<SrvApplicationTableFileDto> UploadFiles(params StorageFile[] files);
        StorageFile DownloadFile(Guid fileId);
        void Delete(long id);
        void DeleteFile(Guid fileId);
        byte[] DownloadPdf(Guid id2, string? lang);
        void Received(RecievedStatusSrvApplicationDto dto);
        Stream PrinSrvApplicationExcel(SrvApplicationSortFilterOptions dto);
        SrvStatisticsDto GetStatisticsDto();
    }
}