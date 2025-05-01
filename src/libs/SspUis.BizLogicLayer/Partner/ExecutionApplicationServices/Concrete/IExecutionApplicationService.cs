using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.ExecutionApplicationServices
{
    public interface IExecutionApplicationService
    : IBaseEntityService<long, ExecutionApplication, ExecutionApplicationListDto, ExecutionApplicationDto, CreateExecutionApplicationDlDto, UpdateExecutionApplicationDlDto, ExecutionApplicationSortFilterOptions>
    {
        PagedResult<ExecutionApplicationListDto> GetList(ExecutionApplicationSortFilterOptions options);
        ExecutionApplicationDto Get();
        Task<HaveId<long>> Create(CreateExecutionApplicationDlDto dto);
        void Cancel(CancelStatusExecutionApplicationDto dto);
        void Accept(AcceptStatusExecutionApplicationDto dto);
        Task Sign(SignStatusExecutionApplicationDto dto);
        public ValueTask<byte[]> DownloadPdf(Guid id2, string? lang);
        List<ExecutionApplicationCellDto> GetFromGraph(int year, int month);
    }
}