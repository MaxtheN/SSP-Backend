using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer
{
    public class SrvApplicationDto : UpdateServiceApplicationDlDto,
        ILinkToEntity<ServiceApplication>,
        IBaseApplication<SspUis.BizLogicLayer.ApplicationDto>
    {
        public new long Id { get => base.Id; set => base.Id = value; }
        public new SspUis.BizLogicLayer.ApplicationDto Application { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Organization { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string? Message { get; set; }
        public bool CanEdit { get; set; }
        public bool CanAccept { get; set; }
        public bool CanSend { get; set; }
        public bool CanDelete { get; set; }
        public bool CanCancel { get; set; }
        public bool CanReject { get; set; }
        public bool CanReceived { get; set; }
        public bool CanCreateContract { get; set; }
        public new bool ToRegionalOffice { get; set; }
        public new List<SrvApplicationGroupDto> Groups { get; set; } = new();
    }
}