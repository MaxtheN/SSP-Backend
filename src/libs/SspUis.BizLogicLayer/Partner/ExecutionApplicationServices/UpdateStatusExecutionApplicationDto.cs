using SspUis.Core;
using SspUis.DataLayer.Repositories;
using System;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.ExecutionApplicationServices
{
    public class UpdateStatusExecutionApplicationDto : UpdateStatusExecutionApplicationDlDto
    { }

    public class AcceptStatusExecutionApplicationDto : UpdateStatusExecutionApplicationDlDto
    {
        public AcceptStatusExecutionApplicationDto()
        {
            base.StatusId = StatusIdConst.ACCEPTED;
        }

        new public int StatusId { get => base.StatusId; }
    }

    public class CancelStatusExecutionApplicationDto : UpdateStatusExecutionApplicationDlDto
    {
        public CancelStatusExecutionApplicationDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }
        new public int StatusId { get => base.StatusId; }
    }
    public class SignStatusExecutionApplicationDto : UpdateStatusExecutionApplicationDlDto
    {
        public SignStatusExecutionApplicationDto()
        {
            base.StatusId = StatusIdConst.SIGNED;
        }

        [LocalizedRequired]
        public string SignedData { get; set; }
        internal Guid SignFile { get; set; }
        internal Guid DataFile { get; set; }
        public DateTime SignedAt { get; set; }
        internal string SignedUserInfo { get; set; }
    }
}
