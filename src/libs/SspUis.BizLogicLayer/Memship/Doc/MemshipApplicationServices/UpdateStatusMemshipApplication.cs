using System;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.MemshipApplicationServices
{
    public class UpdateStatusMemshipApplication : UpdateStatusMemshipApplicationDlDto
    {
    }

    public class AcceptStatusMemshipApplicationDto : UpdateStatusMemshipApplicationDlDto
    {
        public AcceptStatusMemshipApplicationDto()
        {
            base.StatusId = StatusIdConst.ACCEPTED;
        }

        new public int StatusId { get => base.StatusId; }
    }

    public class RejectStatusMemshipApplicationDto : UpdateStatusMemshipApplicationDlDto
    {
        public RejectStatusMemshipApplicationDto()
        {
            base.StatusId = StatusIdConst.REJECTED;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class CancelStatusMemshipApplicationDto : UpdateStatusMemshipApplicationDlDto
    {
        public CancelStatusMemshipApplicationDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }
        //new public int StatusId { get => base.StatusId; }
    }

    public class SendStatusMemshipApplicationDto : UpdateStatusMemshipApplicationDlDto
    {
        public SendStatusMemshipApplicationDto()
        {
            base.StatusId = StatusIdConst.SENT_FOR_REVIEW;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class RevokeStatusMemshipApplicationDto : UpdateStatusMemshipApplicationDlDto
    {
        public RevokeStatusMemshipApplicationDto()
        {
            base.StatusId = StatusIdConst.REVOKED;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class SignStatusMemshipApplicationDto : UpdateStatusMemshipApplicationDlDto
    {
        public SignStatusMemshipApplicationDto()
        {
            base.StatusId = StatusIdConst.SIGNING;
            IsRead = false;
        }
        [LocalizedRequired]
        public string SignedData { get; set; }
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
        internal Guid SignFile { get; set; }
        internal Guid DataFile { get; set; }
        internal string SignedUserInfo { get; set; }
        public override void UpdateEntity(MemshipApplication entity)
        {
            //base.UpdateEntity(entity);
            entity.SignedUserInfo = SignedUserInfo;
            entity.SignedData = SignedData;
            entity.SignFile = SignFile;
            entity.DataFile = DataFile;
            entity.Application.StatusId = StatusId;
            entity.IsRead = false;
        }
    }
}
