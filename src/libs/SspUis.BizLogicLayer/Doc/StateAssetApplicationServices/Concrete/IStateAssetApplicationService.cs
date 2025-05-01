using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DavAktiv;
using System;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ApplicationServices
{
    public interface IStateAssetApplicationService : IBaseEntityService<long, Application, StateAssetApplicationListDto, StateAssetApplicationDto, CreateStateAssetApplicationDlDto, CreateApplicationDlDto, UpdateStateAssetApplicationDlDto, UpdateApplicationDlDto, StateAssetDocumentSortFilterOptions>
    {
        void Accept(AcceptStatusStateAssetApplicationDto dto);
        void Cancel(CancelStatusStateAssetApplicationDto dto);
        bool CanCreate(string inn = null);
        int GetCount();
        StateAssetApplicationDto Get(Guid id2);
        string GetAsHtml();
        string GetAsHtml(long id);
        string GetAsHtml(Guid id2);
        string GetAsHtml(StateAssetApplicationDto dto);
        byte[] GetAsPdf(Guid id2);
        void Reject(RejectStatusStateAssetApplicationDto dto);
        Task Send(SendStatusStateAssetApplicationDto dto);
        Task SendToDavAktiv(long id);
        DavAktivApplicationResponseDto UpdateStateAssetStatus(UpdateStateAssetStatusStateAssetApplicationDto dto);
        void SetModifiedStatus(ModifiedStatusStateAssetApplicationDto dto);

    }
}