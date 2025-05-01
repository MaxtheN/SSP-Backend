using System;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public interface IArbitrationResultService :
    IBaseEntityService<long, ArbitrationResult,
        ArbitrationResultListDto,
        ArbitrationResultDto,
        CreateArbitrationResultDlDto,
        UpdateArbitrationResultDlDto,
        ArbitrationResultSortFilterOptions>
{
    void Cancel(CancelStatusArbitrationResultDto dTo);
    void DeleteFile(Guid fileId);
    StorageFile DownloadFile(Guid fileId);
    Task<ArbitrationResultDto> GetByArbitrationCourtApplicationId(long arbitrationCourtApplicationId);
    Task<HaveId<long>> Sign(SignStatusArbitrationResultDto dto);
    object UploadFiles(StorageFile[] dto);
}
