using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer
{
    public class UpdateStatusSrvContract : UpdateStatusServiceContractDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
        public override void UpdateEntity(ServiceContract entity)
        {
            base.UpdateEntity(entity);
            entity.StatusId = StatusId;
            entity.Details = Details;
        }
    }

    public class SignStatusSrvContractDto : UpdateStatusSrvContract
    {
        public SignStatusSrvContractDto()
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
    public class SigningStatusSrvContractDto : UpdateStatusSrvContract
    {
        public SigningStatusSrvContractDto()
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
    public class RejectStatusSrvContractDto : UpdateStatusSrvContract
    {
        public RejectStatusSrvContractDto()
        {
            base.StatusId = StatusIdConst.REJECTED;
        }
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
    }
    public class CancelStatusSrvContractDto : UpdateStatusSrvContract
    {
        public CancelStatusSrvContractDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
    }
    public class DeletetatusSrvContractDto : UpdateStatusSrvContract
    {
        public DeletetatusSrvContractDto()
        {
            base.StatusId = StatusIdConst.DELETED;
        }
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
    }
}
