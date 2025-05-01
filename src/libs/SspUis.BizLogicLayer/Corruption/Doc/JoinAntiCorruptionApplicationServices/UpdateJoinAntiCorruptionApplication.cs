using SspUis.Core;
using SspUis.DataLayer.Repositories;
using System;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.JoinAntiCorruptionApplicationServices
{
    public class UpdateJoinAntiCorruptionApplication : UpdateStatusJoinAntiCorruptionApplicationDlDto
    {
    }

    public class AcceptStatusJoinAntiCorruptionApplicationDto : UpdateStatusJoinAntiCorruptionApplicationDlDto
    {
        public AcceptStatusJoinAntiCorruptionApplicationDto()
        {
            base.StatusId = StatusIdConst.ACCEPTED;
        }
        new public int StatusId { get => base.StatusId; set => base.StatusId = value; }
    }

    public class RejectStatusJoinAntiCorruptionApplicationDto : UpdateStatusJoinAntiCorruptionApplicationDlDto
    {
        public RejectStatusJoinAntiCorruptionApplicationDto()
        {
            base.StatusId = StatusIdConst.REJECTED;
        }
        new public int StatusId { get => base.StatusId; set => base.StatusId = value; }
    }

    public class CancelStatusJoinAntiCorruptionApplicationDto : UpdateStatusJoinAntiCorruptionApplicationDlDto
    {
        public CancelStatusJoinAntiCorruptionApplicationDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class SendStatusJoinAntiCorruptionApplicationDto : UpdateStatusJoinAntiCorruptionApplicationDlDto
    {
        public SendStatusJoinAntiCorruptionApplicationDto()
        {
            base.StatusId = StatusIdConst.SENT;
        }
        [LocalizedRequired]
        public string SignedData { get; set; }
        public bool IsPinfl { get; set; } = false;
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
        internal Guid SignFile { get; set; }
        internal Guid DataFile { get; set; }
        internal string SignedUserInfo { get; set; }
    }

    public class RevokeStatusJoinAntiCorruptionApplicationDto : UpdateStatusJoinAntiCorruptionApplicationDlDto
    {
        public RevokeStatusJoinAntiCorruptionApplicationDto()
        {
            base.StatusId = StatusIdConst.REVOKED;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class SignStatusJoinAntiCorruptionApplicationDto : UpdateStatusJoinAntiCorruptionApplicationDlDto
    {
        public SignStatusJoinAntiCorruptionApplicationDto()
        {
            base.StatusId = StatusIdConst.SIGNED;
        }
        new public int StatusId { get => base.StatusId; }
    }
}
