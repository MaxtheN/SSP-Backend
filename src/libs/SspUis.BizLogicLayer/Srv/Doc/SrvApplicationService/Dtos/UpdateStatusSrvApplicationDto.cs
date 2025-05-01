using SspUis.Core;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer
{
    public class AcceptStatusSrvApplicationDto : UpdateStatusServiceApplicationDlDto
    {
        public AcceptStatusSrvApplicationDto()
        {
            base.StatusId = StatusIdConst.ACCEPTED;
        }

        new public int StatusId { get => base.StatusId; }
    }
    public class RecievedStatusSrvApplicationDto : UpdateStatusServiceApplicationDlDto
    {
        public RecievedStatusSrvApplicationDto()
        {
            base.StatusId = StatusIdConst.RECEIVED;
        }

        new public int StatusId { get => base.StatusId; set => base.StatusId = value; }

        public List<ServiceApplicationGroupIsCompletedDlDto> Groups { get; set; } = new();
    }

    public class RejectStatusSrvApplicationDto : UpdateStatusServiceApplicationDlDto
    {
        public RejectStatusSrvApplicationDto()
        {
            base.StatusId = StatusIdConst.REJECTED;
        }
        new public int StatusId { get => base.StatusId; }


    }

    public class CancelStatusSrvApplicationDto : UpdateStatusServiceApplicationDlDto
    {
        public CancelStatusSrvApplicationDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class SendStatusSrvApplicationDto : UpdateStatusServiceApplicationDlDto
    {
        public SendStatusSrvApplicationDto()
        {
            base.StatusId = StatusIdConst.SENT;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class RevokeStatusSrvApplicationDto : UpdateStatusServiceApplicationDlDto
    {
        public RevokeStatusSrvApplicationDto()
        {
            base.StatusId = StatusIdConst.SENT;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class SignStatusSrvApplicationDto : UpdateStatusServiceApplicationDlDto
    {
        public SignStatusSrvApplicationDto()
        {
            base.StatusId = StatusIdConst.SIGNING;
        }

        [LocalizedRequired]
        public string SignedData { get; set; }
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
        internal Guid SignFile { get; set; }
        internal Guid DataFile { get; set; }
        internal string SignedUserInfo { get; set; }
    }
}