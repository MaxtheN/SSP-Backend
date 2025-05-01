using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Memship;

public interface IMemshipPaymentOrderService : IBaseEntityService<long, MemshipPaymentOrder, MemshipPaymentOrderListDto, MemshipPaymentOrderDto, CreateMemshipPaymentOrderDlDto, UpdateMemshipPaymentOrderDlDto, MemshipPaymentOrderSortFilterOption>
{
    MemshipPaymentOrderDto GetByMemshipContractId(long memshipContractId);
    HaveId<long> Accept(long id);
    HaveId<long> Cancel(long id, string message);
    IEnumerable<MemshipPaymentOrderFileDto> UploadFiles(params StorageFile[] files);
    StorageFile DownloadFile(Guid fileId);
    void DeleteFile(Guid fileId);
    Stream SaveAsExcelPrtnBojxonaContracts(MemshipPaymentOrderSortFilterOption options);
}
