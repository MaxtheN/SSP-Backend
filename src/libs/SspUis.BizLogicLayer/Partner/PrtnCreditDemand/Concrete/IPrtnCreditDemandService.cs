using SspUis.BizLogicLayer.Doc.PrtnContractServices;
using SspUis.BizLogicLayer.PrtnCertificateServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.PrtnCreditDemandServices
{
    public interface IPrtnCreditDemandService
        : IBaseEntityService<long, PrtnCreditDemand, PrtnCreditDemandListDto, PrtnCreditDemandDto, CreatePrtnCreditDemandDlDto, UpdatePrtnCreditDemandDlDto, PrtnCreditDemandSortFilterOptions>
    {
        SelectList<long> AsSelectList();
   //     //PrtnCreditDemandDto GetByPrtnContractId(long prtnContractId);
   //     void Cancel(CancelStatusPrtnCreditDemandDto dto);
 		Stream SaveAsExecel(PrtnCreditDemandSortFilterOptions dto);
        int GetCount();
        PrtnCreditDemandDto GetCheckCreditDemandWithCertificate(Guid? id2, string contractorInn);
        
        //Stream PrinGraphExcel(PrtnDocumentSortFilterOptions dto);
        //     string GetHtmlTemplate(PrtnCreditDemandDto dto);
        //     byte[] GetPdfTemplate(PrtnCreditDemandDto dto);
        //     byte[] GetPdfTemplateByPrtnContractId(long prtnContractId);
    }
}
