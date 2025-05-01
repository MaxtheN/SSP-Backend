using SspUis.BizLogicLayer.Doc.PrtnContractServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.PrtnContractServices
{
    public interface IPrtnContractService
        : IBaseEntityService<long, PrtnContract, PrtnContractListDto, PrtnContractDto, CreatePrtnContractDlDto, UpdatePrtnContractDlDto, PrtnDocumentSortFilterOptions>
    {
        SelectList<long> AsSelectList();
        PrtnContractDto GetByApplicationId(long applicationId);
        Task Sign(SignStatusPrtnContractDto dto);
        void Revoke(RevokeStatusPrtnContractDto dto);
        Task PassExpertise(PassExpertiseStatusPrtnContractDto dto);
        Task ResendExpertise(ResendExpertiseStatusPrtnContractDto dto);
        void NotPassExpertise(NotPassExpertiseStatusPrtnContractDto dto);
        IEnumerable<PrtnContractFileDto> UploadFiles(params StorageFile[] files);
        StorageFile DownloadFile(Guid fileId);
        int GetCount();
        void DeleteFile(Guid fileId);
        void Reject(RejectStatusPrtnContractDto dto);
        void Cancel(CancelStatusPrtnContractDto dto);
        Stream SaveAsExecel(PrtnDocumentSortFilterOptions dto);
        string GetHtmlTemplate(PrtnContractDto dto);
        byte[] GetPdfTemplate(long applicationId);
        IQueryable<PrtnContractListDto> GetPrtnContractListDto(PrtnDocumentSortFilterOptions dto);
         Task InsertPrtnContractDataBase();
        Task InsertPrtnCertifcatCulumn();
    }
}
