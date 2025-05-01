using SspUis.DataLayer;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbImzo.Models;

namespace SspUis.BizLogicLayer.Doc.SignDocumentManage;

public interface ISignDocumentManageService : IStatusGeneric
{
    Task<bool> PostSignResponse(WbImzoSignInformDto dto);
    Task GetErrors();
    Task<WbImzoSignInformResponseDto> PostSignResponse2(WbImzoSignInformDto dto);
    Task UpdateStatus(UpdateStatusWebImzoDto dto);
}
