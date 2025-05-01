using System;
using System.Threading.Tasks;
using SspUis.BizLogicLayer.Arbitration.Doc.ArbitrationDiscussionServices.QueryObjects;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public interface IArbitrationDiscussionService :
    IBaseEntityService<long, ArbitrationDiscussion,
        ArbitrationDiscussionListDto,
        ArbitrationDiscussionDto,
        CreateArbitrationDiscussionDlDto,
        UpdateArbitrationDiscussionDlDto,
		ArbitrationDiscussionSortFilterOptions>
{
	Task<ArbitrationDiscussionDto> GetByArbitrationCourtApplicationId(int arbitrationCourtApplicationId);
	Task<HaveId<long>> Sign(SignStatusArbitrationDiscussionDto dto);
    Task<HaveId<long>> Update(UpdateArbitrationDiscussionDlDto dto);

    #region Files
    object UploadFiles(StorageFile[] dto);
	void DeleteFile(Guid fileId);
	StorageFile DownloadFile(Guid fileId);
	Task<byte[]> DownloadTemplate();
    #endregion
}
