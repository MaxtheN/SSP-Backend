using SspUis.BizLogicLayer.JoinAntiCorruptionResultServices;
using SspUis.BizLogicLayer.Models;
using SspUis.DataLayer.EfClasses.Corruption;
using SspUis.DataLayer.Repositories.Corruption;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Corruption
{

    public interface IJoinAntiCorruptionResultService : IBaseEntityService<long, JoinAntiCorruptionResult, JoinAntiCorruptionResultListDto, JoinAntiCorruptionResultDto, CreateJoinAntiCorruptionResultDlDto, UpdateJoinAntiCorruptionResultDlDto, JoinAntiCorruptionResultSortFilterOptions>
    {
        PagedResult<JoinAntiCorruptionResultListDto> GetList(JoinAntiCorruptionResultSortFilterOptions dto);
        JoinAntiCorruptionResultDto Get();
        JoinAntiCorruptionResultDto Get(long id);
        SelectList<long> AsSelectList();
        HaveId<long> Create(CreateJoinAntiCorruptionResultDlDto dto);
        Task<HaveId<long>> Accept(UpdateStatusJoinAntiCorruptionResultDto dto);
        HaveId<long> Cancel(UpdateStatusJoinAntiCorruptionResultDto dto);
        void Update(UpdateJoinAntiCorruptionResultDlDto dto);
        void Delete(long id);
        IEnumerable<JoinAntiCorruptionResultFileDto> UploadFiles(params StorageFile[] files);
        StorageFile DownloadFile(Guid fileId);
        void DeleteFile(Guid fileId);
        Task<byte[]> DownloadPdf(Guid id2, string? lang);
        Guid SaveFile(long docId, string data, string fileName);
    }
}
