using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SspUis.BizLogicLayer.Doc.PrtnContractServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.PrtnCertificateServices
{
    public interface IPrtnCertificateService
        : IBaseEntityService<long, PrtnCertificate, PrtnCertificateListDto, PrtnCertificateDto, CreatePrtnCertificateDlDto, UpdatePrtnCertificateDlDto, PrtnDocumentSortFilterOptions>
    {
        SelectList<long> AsSelectList();
        PrtnCertificateDto GetByPrtnContractId(long prtnContractId);
        int GetCount();
        Task Cancel(CancelStatusPrtnCertificateDto dto);
        IntegrationCertificateRequestDto GetCertificateInfo(int? lang, Guid? id2, string contractorInn);
        Stream SaveAsExecel(PrtnDocumentSortFilterOptions dto);
        Stream PrinGraphExcel(PrtnDocumentSortFilterOptions dto);
        string GetHtmlTemplate(PrtnCertificateDto dto);
        byte[] GetPdfTemplate(PrtnCertificateDto dto);
        byte[] GetPdfTemplateByPrtnContractId(long prtnContractId);
        byte[] GetPdfById2(Guid id2);
        byte[] GetPdfById(long id);
        Task SentForOtherServiceFormedCertificate(long id, Enum Type);
        List<long> PostAllOldCertificates(int id);
        IQueryable<PrtnCertificateListDto> GetPrtnCertificateListDto(PrtnDocumentSortFilterOptions dto);
    }
}
