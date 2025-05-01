using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.Models;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Propos
{
    public interface IProposalService : IStatusGeneric
    {
        PagedResult<ProposalListDto> GetList(ProposalSortFilterOptions options);
        ProposalDto Get();
        Task<ContractorDto> GetfromSoliqByInn(string inn);
        ProposalDto Get(long id);
        HaveId<long> Create(CreateProposalDlDto dto);
        void Update(UpdateProposalDlDto dto);
        void Delete(long id);
        IEnumerable<IStorageFileInfo> UploadFiles(params StorageFile[] files);
        Stream SaveAsExcel(ProposalSortFilterOptions dto);
    }
}
