using SspUis.Core;
using SspUis.DataLayer.Repositories;
using System;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.ClaimApplicationServices
{
    public class UpdateStatusClaimApplicationDto : UpdateStatusClaimApplicationDlDto
    { }

    public class AcceptStatusClaimApplicationDto : UpdateStatusClaimApplicationDlDto
    {
        public AcceptStatusClaimApplicationDto()
        {
            base.StatusId = StatusIdConst.ACCEPTED;
        }

        new public int StatusId { get => base.StatusId; }
    }

    public class RejectStatusClaimApplicationDto : UpdateStatusClaimApplicationDlDto
    {
        public RejectStatusClaimApplicationDto()
        {
            base.StatusId = StatusIdConst.REJECTED;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class CancelStatusClaimApplicationDto : UpdateStatusClaimApplicationDlDto
    {
        public CancelStatusClaimApplicationDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class RevokeStatusClaimApplicationDto : UpdateStatusClaimApplicationDlDto
    {
        public RevokeStatusClaimApplicationDto()
        {
            base.StatusId = StatusIdConst.REVOKED;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class SendStatusClaimApplicationDto : UpdateStatusClaimApplicationDlDto
    {
        public SendStatusClaimApplicationDto()
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
