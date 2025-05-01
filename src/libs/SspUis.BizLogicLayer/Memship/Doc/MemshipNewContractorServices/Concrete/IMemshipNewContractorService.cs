using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Memship;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Memship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Memship;

public interface IMemshipNewContractorService : IBaseEntityService<long, MemshipNewContractor, MemshipNewContractorListDto, MemshipNewContractorDto, CreateMemshipNewContractorDlDto, UpdateMemshipNewContractorDlDto, MemshipNewContractorSortFilterOptions>
{
    PagedResult<MemshipNewContractorListDto> GetList(MemshipNewContractorSortFilterOptions options);
    MemshipNewContractorDto Get();
    List<MemshipNewContractorTableDto> FillTable(int regionId);
    MemshipNewContractorDto Get(long id);
    SelectList<long> AsSelectList();
    HaveId<long> Create(CreateMemshipNewContractorDlDto dto);
    void Accept(UpdateStatusMemshipNewContractorDto dTo);
    void Cancel(UpdateStatusMemshipNewContractorDto dTo);
    void Update(UpdateMemshipNewContractorDlDto dto);
    void Delete(long id);
    IEnumerable<MemshipNewContractorFileDto> UploadFiles(params StorageFile[] files);
    StorageFile DownloadFile(Guid fileId);
    void DeleteFile(Guid fileId);
}

