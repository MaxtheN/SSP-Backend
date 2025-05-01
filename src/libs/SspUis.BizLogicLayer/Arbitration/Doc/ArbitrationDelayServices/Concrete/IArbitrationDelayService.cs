using System;
using System.Threading.Tasks;
using SspUis.BizLogicLayer.Arbitration.Doc.ArbitrationDelayServices.QueryObjects;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public interface IArbitrationDelayService :
    IBaseEntityService<long, ArbitrationDelay,
        ArbitrationDelayListDto,
        ArbitrationDelayDto,
        CreateArbitrationDelayDlDto,
        UpdateArbitrationDelayDlDto,
		ArbitrationDelaySortFilterOptions>
{
	Task<ArbitrationDelayDto> GetByArbitrationCourtApplicationId(int arbitrationCourtApplicationId);
	Task<HaveId<long>> Sign(SignStatusArbitrationDelayDto dto);
    Task<HaveId<long>> Update(UpdateArbitrationDelayDlDto dto);

    #region Files
    object UploadFiles(StorageFile[] dto);
	void DeleteFile(Guid fileId);
	StorageFile DownloadFile(Guid fileId);
	Task<byte[]> DownloadTemplate();
    #endregion
}
