using SspUis.BizLogicLayer.MemshipApplicationServices;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Collections.Generic;
using System;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Hrm.NeedChamberServiceServices
{
    public interface INeedChamberServiceService : IStatusGeneric
    {
        PagedResult<NeedChamberServiceListDto> GetList(NeedChamberServiceSortFilterDto dto);
        List<NeedChamberServiceGroupAndChildListDto> GetListGroupAndChild(bool isPaid);

		NeedChamberServiceDto Get();
        NeedChamberServiceDto Get(int id);
        SelectList<int> AsSelectList(int? groupId);
        HaveId<int> Create(CreateNeedChamberServiceDlDto dto);
        void Update(UpdateNeedChamberServiceDlDto dto);
        void Delete(int id);
        IEnumerable<NeedChamberServiceFileDto> UploadFiles(params StorageFile[] files);
        StorageFile DownloadFile(Guid fileId);
        void DeleteFile(Guid fileId);
        Dictionary<int, bool> WithIsOfferta();
        List<NeedChamberServiceGroupingDto> GroupingByFreeServices(int? groupId);
    }
}
