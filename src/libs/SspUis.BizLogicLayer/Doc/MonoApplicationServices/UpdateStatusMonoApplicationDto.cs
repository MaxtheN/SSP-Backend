using SspUis.Core;
using SspUis.DataLayer.Repositories;
using System;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.MonoApplicationServices
{
    public class UpdateStatusMonoApplicationDto : UpdateStatusMonoApplicationDlDto
    { }

    public class AcceptStatusMonoApplicationDto : UpdateStatusMonoApplicationDlDto
    {
        public AcceptStatusMonoApplicationDto()
        {
            base.StatusId = StatusIdConst.ACCEPTED;
        }

        new public int StatusId { get => base.StatusId; }
    }

    public class RejectStatusMonoApplicationDto : UpdateStatusMonoApplicationDlDto
    {
        public RejectStatusMonoApplicationDto()
        {
            base.StatusId = StatusIdConst.REJECTED;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class CancelStatusMonoApplicationDto : UpdateStatusMonoApplicationDlDto
    {
        public CancelStatusMonoApplicationDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class RevokeStatusMonoApplicationDto : UpdateStatusMonoApplicationDlDto
    {
        public RevokeStatusMonoApplicationDto()
        {
            base.StatusId = StatusIdConst.REVOKED;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class SendStatusMonoApplicationDto : UpdateStatusMonoApplicationDlDto
    {
        public SendStatusMonoApplicationDto()
        {
            base.StatusId = StatusIdConst.SENT;
        }
        [LocalizedRequired]
        public bool IsPinfl { get; set; } = false;
        public string SignedData { get; set; }
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
        internal Guid SignFile { get; set; }
        internal Guid DataFile { get; set; }
        internal string SignedUserInfo { get; set; }
    }
}
