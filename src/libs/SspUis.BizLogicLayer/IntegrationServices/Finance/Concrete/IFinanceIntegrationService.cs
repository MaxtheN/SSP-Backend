using DocumentFormat.OpenXml.Office2010.ExcelAc;

using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.NotificationServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Exapidata.Finance;
using SspUis.Integration.Finance.Models;
using StatusGeneric;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.IntegrationServices.Finance.Concrete;
public interface IFinanceIntegrationService : IStatusGenericHandler
{
    public PagedResult<FinPaymentData> GetFinancePaymentDataList( FinPaymentDataSortFilterPageOptions dto);
    public PagedResult<FinancePayDocsByAcc> GetFinancePayDocsByAccList(FinancePayDocsByAccSortFilterPageOptions dto);
    public PagedResult<FinancePaymentByInnDto> GetFinancePaymentByInn(FinancePaymentByInnDtoSortFilterPageOptions dto);
    HaveId<long> CreateFinancePymentData(FinPaymentDataDto dto);
    void SaveFinancePayDocsByAcc(List<GetPayDocsDto> data);


}
