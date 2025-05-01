using SspUis.BizLogicLayer.Doc.ApplicationServices;
using SspUis.BizLogicLayer.Doc.PrtnContractServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.OnlineMahalla;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ApplicationServices
{
    public interface IApplicationService
        : IBaseEntityService<long, Application, ApplicationListDto, ApplicationDto, CreateApplicationDlDto, UpdateApplicationDlDto, PrtnDocumentSortFilterOptions>
    {
        void Accept(long id);
        SelectList<long> AsSelectList();
        Task<PrtnApplicationDto> GetPrtnApplication();
        PrtnApplicationDto GetPrtnApplication(long id);
        HaveId<long> CreatePrtnApplication(CreatePrtnApplicationDlDto dto);
        Boolean CanCreateApplication(string inn);
        void UpdatePrtnApplication(UpdatePrtnApplicationDlDto dto);
        void Accept(AcceptStatusPrtnApplicationDto dlDto);
        void Complete(long Id);
        ApplicationVacanсiesDto GetApplicationVacancies();
        Task Reject(RejectStatusPrtnApplicationDto dlDto);
        void Cancel(CancelStatusPrtnApplicationDto dlDto);
        Task SentForReview(long id, string userIp = null, string userAgent = null);
        Task SentToMahalla(long id, string userIp = null, string userAgent = null);
        List<AvailableBenefits> GetAvailableBenefits();
        MemshipPaymentsInfo GetMemshipPayments();
        void Revoke(long id);
        Stream SaveAsExecel(PrtnDocumentSortFilterOptions dto);
        HaveId<long> ChangeStatusMfyApplication(MfyApplicationStateDto dto);
        CheckApplicationStatusDto CheckApplicationStatus(string applicationId);
        MfyApplicationLogDto GetMfyApplication(Guid applicationId2);
        IQueryable<ApplicationListDto> GetListMethod(PrtnDocumentSortFilterOptions options);
        int GetCount();
        List<long> PostAllOldMfyApplications();
        void AddPrtnApplicationStatusChangeExpOn();
        List<ApplicationNotificationDto> GetApplicationNotification();
        public bool IsRead(string typeName, long Id);
    }
}
