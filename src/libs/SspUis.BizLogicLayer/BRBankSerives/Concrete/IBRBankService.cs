using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.BizLogicLayer.Appeal;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.DataLayer.Repositories.Appeal;
using SspUis.Integration.DigitizationCenter.Models.GSP;
using StatusGeneric;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.BRBankSerives.Concrete
{
    public interface IBRBankService : IStatusGeneric
    {
        Task<BRBankResponseDto> GetResult(CreateAppealApplicationDlDto dto);
        Task<PersonDto> GetByPassportDataFromDigital(BrBankPersonDlDto dto);
        AppealApplicationDto Get();
        IEnumerable<AppealApplicationFileDto> UploadFiles(params StorageFile[] files);
        StorageFile DownloadFile(Guid fileId);
        void DeleteFile(Guid fileId);
    }
}
