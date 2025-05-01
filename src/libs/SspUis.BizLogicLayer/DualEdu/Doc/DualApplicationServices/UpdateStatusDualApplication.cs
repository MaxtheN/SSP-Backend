using System;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.DataLayer.Repositories;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.DualApplicationServices
{
    public class UpdateStatusDualApplication : UpdateStatusDualApplicationDlDto
    {
    }

    public class AcceptStatusDualApplicationDto : UpdateStatusDualApplicationDlDto
    {
        public AcceptStatusDualApplicationDto()
        {
            base.StatusId = StatusIdConst.ACCEPTED;
        }

        new public int StatusId { get => base.StatusId; }
    }

    public class RejectStatusDualApplicationDto : UpdateStatusDualApplicationDlDto
    {
        public RejectStatusDualApplicationDto()
        {
            base.StatusId = StatusIdConst.REJECTED;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class CancelStatusDualApplicationDto : UpdateStatusDualApplicationDlDto
    {
        public CancelStatusDualApplicationDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class SendStatusDualApplicationDto : UpdateStatusDualApplicationDlDto
    {
        public SendStatusDualApplicationDto()
        {
            StatusId = StatusIdConst.SENT;
        }
        [LocalizedRequired]
        public string SignedData { get; set; }
        public bool IsPinfl { get; set; } = false;
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
        internal Guid SignFile { get; set; }
        internal Guid DataFile { get; set; }
        internal string SignedUserInfo { get; set; }
        public override void UpdateEntity(DualApplication entity)
        {
            base.UpdateEntity(entity);
            entity.Application.StatusId = StatusId;
        }
        //public override void UpdateEntity(Application entity)
        //{
        //    base.UpdateEntity(entity);
        //    //entity.DualApplication.SignedUserInfo = SignedUserInfo;
        //    //entity.DualApplication.SignedData = SignedData;
        //    //entity.DualApplication.SignFile = SignFile;
        //    //entity.DualApplication.DataFile = DataFile;
        //    entity.StatusId = StatusId;
        //}
    }

    public class RevokeStatusDualApplicationDto : UpdateStatusDualApplicationDlDto
    {
        public RevokeStatusDualApplicationDto()
        {
            base.StatusId = StatusIdConst.SENT;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class SignStatusDualApplicationDto : UpdateStatusDualApplicationDlDto
    {
        public SignStatusDualApplicationDto()
        {
            base.StatusId = StatusIdConst.SIGNING;
        }
        new public int StatusId { get => base.StatusId; }
    }
}
