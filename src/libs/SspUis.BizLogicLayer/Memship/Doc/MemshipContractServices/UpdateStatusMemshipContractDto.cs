using System;
using System.Collections.Generic;
using System.Linq;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Memship;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.BizLogicLayer.Memship
{
    public class UpdateStatusMemshipContractDto : UpdateStatusMemshipContractDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
    }
    public class SignStatusMemshipContractDto : UpdateStatusMemshipContractDto
    {
        [LocalizedRequired]
        public string SignedData { get; set; }
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
        internal Guid SignFile { get; set; }
        internal Guid DataFile { get; set; }
        internal string SignedUserInfo { get; set; }
        public bool IsPinfl { get; set; } = false;
    }
    public class RejectStatusMemshipContractDto : UpdateStatusMemshipContractDto
    {
        public RejectStatusMemshipContractDto()
        {
            base.StatusId = StatusIdConst.REJECTED;
        }
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
        public List<MemshipContractFileDto> Files { get; set; }

        public override void UpdateEntity(MemshipContract entity)
        {
            base.UpdateEntity(entity);
            entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_MEMSHIP_CONTRACT_FILE, Files.Select(a => a.Id).ToList());
        }

        //[LocalizedRequired]
        //public string Message { get; set; } = null!;
        //[LocalizedRequired]
        //[LocalizedRange(1, int.MaxValue)]
        //public int MemshipRejectReasonId { get; set; }
    }
    public class CancelStatusMemshipContractDto : UpdateStatusMemshipContractDto
    {
        public CancelStatusMemshipContractDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
    }
}
