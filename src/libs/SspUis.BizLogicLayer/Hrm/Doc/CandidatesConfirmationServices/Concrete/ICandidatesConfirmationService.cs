using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer
{
    public interface ICandidatesConfirmationService : IBaseEntityService<
            long,
            CandidatesConfirmation,
            CandidatesConfirmationListDto,
            CandidatesConfirmationDto,
            CreateCandidatesConfirmationDlDto,
            UpdateCandidatesConfirmationDlDto,
            CandidatesConfirmationSortFilterPageOption>
    {
        void Send(UpdateStatusCandidatesConfirmationDto dTo);
        void DeleteFile(Guid fileId);
        void Accept(UpdateStatusCandidatesConfirmationDto dTo);
        void Cancel(CancelStatusCandidatesConfirmationDto dTo);
        void ReceiveTable(UpdateStatusCandidatesConfirmationTableDto dTo);
        void RejectTable(CancelStatusCandidatesConfirmationTableDto dTo);
        StorageFile DownloadFile(Guid fileId);
        IEnumerable<IStorageFileInfo> UploadFiles(params StorageFile[] files);
    }
}
