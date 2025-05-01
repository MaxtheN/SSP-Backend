using SspUis.DataLayer.Repositories;
using SspUis.Integration.Sud.Models;
using SspUis.Integration.Sud.Models.AuthModels;
using SspUis.Integration.Sud.Models.MalumotnomaModel;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud.Services;

public interface ISudService : IStatusGenericHandler
{
    Task<object> SudSendingNewClaim(SendingNewClaimDto sendingNewClaimDto, CreateCourtntegrationDlDto dto);
    Task<UploadFileResponseDto> SudUploadFile(UploadFileDto dto);

    Task<InvoiceResponseModel> SudInvoice(InvoiceModel invoiceModel);

    #region Справочник
    Task<List<CommonEntity>> GetSudCourtList();
    Task<List<CommonEntity>> GetSudDutyReasonList();
    Task<List<CommonEntity>> GetSudPostReasonList();
    Task<List<CommonEntity>> GetSudCategoryList();
    Task<List<CommonEntity>> GetSudCategoriesSubList();
    Task<List<CommonEntity>> GetSudCategoriesSecondList();
    Task<List<CurrencyModel>> GetSudCurrencyList();
    Task<List<CommonEntity>> GetSudDocumentTypesList();
    Task<List<string>> GetSudParticipantTypeList();
    Task<List<string>> GetSudEntityTypeList();
    Task<List<CommonEntityRegion>> GetSudRegionList();
    Task<List<CommonEntityRegion>> GetSudDistrictList(Guid regionId);
    Task<List<CommonEntity>> GetSudCountryList();
    Task<List<CommonEntity>> GetSudAmountCategoryList();
    Task<List<string>> GetSudClaimKindList();
    Task<List<BankModel>> GetSudBankList();
    #endregion
}

