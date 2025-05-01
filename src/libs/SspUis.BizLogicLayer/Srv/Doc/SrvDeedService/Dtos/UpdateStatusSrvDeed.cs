using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer
{
    public class UpdateStatusSrvDeed : UpdateStatusServiceDeedDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
        public override void UpdateEntity(ServiceDeed entity)
        {
            base.UpdateEntity(entity);
            entity.StatusId = StatusId;
            entity.Details = Details;
        }
    }

    public class SignStatusSrvDeedDto : UpdateStatusSrvDeed
    {
        public SignStatusSrvDeedDto()
        {
            base.StatusId = StatusIdConst.SIGNED;
        }

        [LocalizedRequired]
        public string SignedData { get; set; }
        public bool IsPinfl { get; set; } = false;
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
        internal Guid SignFile { get; set; }
        internal Guid DataFile { get; set; }
        internal string SignedUserInfo { get; set; }
    }
    public class SigningStatusSrvDeedDto : UpdateStatusSrvDeed
    {
        public SigningStatusSrvDeedDto()
        {
            base.StatusId = StatusIdConst.SIGNING;
        }

        [LocalizedRequired]
        public string SignedData { get; set; }
        public bool IsPinfl { get; set; } = false;
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
        internal Guid SignFile { get; set; }
        internal Guid DataFile { get; set; }
        internal string SignedUserInfo { get; set; }
    }
    public class RejectStatusSrvDeedDto : UpdateStatusSrvDeed
    {
        public RejectStatusSrvDeedDto()
        {
            base.StatusId = StatusIdConst.REJECTED;
        }
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
    }
    public class CancelStatusSrvDeedDto : UpdateStatusSrvDeed
    {
        public CancelStatusSrvDeedDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
    }
    public class DeletetatusSrvDeedDto : UpdateStatusSrvDeed
    {
        public DeletetatusSrvDeedDto()
        {
            base.StatusId = StatusIdConst.DELETED;
        }
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
    }
}
