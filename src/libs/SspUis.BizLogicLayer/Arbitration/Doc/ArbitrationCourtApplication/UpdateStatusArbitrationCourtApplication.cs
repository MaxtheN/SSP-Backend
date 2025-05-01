using System;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.ArbitrationCourtApplicationServices
{
    public class UpdateStatusArbitrationCourtApplication : UpdateStatusArbitrationCourtApplicationDlDto
    {
    }

    public class AcceptStatusArbitrationCourtApplicationDto : UpdateStatusArbitrationCourtApplicationDlDto
    {
        public AcceptStatusArbitrationCourtApplicationDto()
        {
            base.StatusId = StatusIdConst.ACCEPTED;
        }

        new public int StatusId { get => base.StatusId; }
    }

    public class RejectStatusArbitrationCourtApplicationDto : UpdateStatusArbitrationCourtApplicationDlDto
    {
        public RejectStatusArbitrationCourtApplicationDto()
        {
            base.StatusId = StatusIdConst.REJECTED;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class CancelStatusArbitrationCourtApplicationDto : UpdateStatusArbitrationCourtApplicationDlDto
    {
        public CancelStatusArbitrationCourtApplicationDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class SendStatusArbitrationCourtApplicationDto : UpdateStatusArbitrationCourtApplicationDlDto
    {
        public SendStatusArbitrationCourtApplicationDto()
        {
            base.StatusId = StatusIdConst.SENT_FOR_REVIEW;
        }
        new public int StatusId { get => base.StatusId; }
    }

    //public class RevokeStatusArbitrationCourtApplicationDto : UpdateStatusArbitrationCourtApplicationDlDto
    //{
    //    public RevokeStatusArbitrationCourtApplicationDto()
    //    {
    //        base.StatusId = StatusIdConst.REVOKED;
    //    }
    //    new public int StatusId { get => base.StatusId; }
    //}

    public class SignStatusArbitrationCourtApplicationDto : UpdateStatusArbitrationCourtApplicationDlDto
    {
        public SignStatusArbitrationCourtApplicationDto()
        {
            base.StatusId = StatusIdConst.SIGNING;
        }
        [LocalizedRequired]
        public string SignedData { get; set; }
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
       
    }
}
