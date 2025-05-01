using Newtonsoft.Json;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.ApplicationServices
{
    public class UpdateStatusStateAssetApplication : UpdateStatusApplicationDlDto
    {
    }

    public class AcceptStatusStateAssetApplicationDto : UpdateStatusApplicationDlDto
    {
        public AcceptStatusStateAssetApplicationDto()
        {
            base.StatusId = StatusIdConst.ACCEPTED;
        }

        new public int StatusId { get => base.StatusId; }
    }

    public class ModifiedStatusStateAssetApplicationDto : UpdateStatusApplicationDlDto
    {
        public ModifiedStatusStateAssetApplicationDto()
        {
            base.StatusId = StatusIdConst.MODIFIED;
        }

        new public int StatusId { get => base.StatusId; }
    }

    public class RejectStatusStateAssetApplicationDto : UpdateStatusApplicationDlDto
    {
        public RejectStatusStateAssetApplicationDto()
        {
            base.StatusId = StatusIdConst.REJECTED;
        }

        new public int StatusId { get => base.StatusId; }
    }

    public class CancelStatusStateAssetApplicationDto : UpdateStatusApplicationDlDto
    {
        public CancelStatusStateAssetApplicationDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }

        new public int StatusId { get => base.StatusId; }
    }

    public class SendStatusStateAssetApplicationDto : UpdateStatusApplicationDlDto
    {
        public SendStatusStateAssetApplicationDto()
        {
            base.StatusId = StatusIdConst.SENT_FOR_REVIEW;
        }

        new public int StatusId { get => base.StatusId; }
        public string UserIp { get; set; }
        public string UserAgent { get; set; }

        public override void UpdateEntity(Application entity)
        {
            base.UpdateEntity(entity);
            entity.StateAssetApplication.StateAssetStatusId = StateAssetStatusIdConst.NEW;
        }
    }

    public class UpdateStateAssetStatusStateAssetApplicationDto
    {
        [JsonProperty("id")]
        public Guid Id2 { get; set; }
        [JsonProperty("status")]
        public int StateAssetStatusId { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
