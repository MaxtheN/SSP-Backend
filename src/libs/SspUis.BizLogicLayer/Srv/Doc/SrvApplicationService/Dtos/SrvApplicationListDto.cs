using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer
{
    public class SrvApplicationListDto : DocumentListDto<long>, ILinkToEntity<ServiceApplication>
    {
        public ApplicationListDto Application { get; set; }
        public bool IsFree { get; set; }
        public int TableId { get; } = TableIdConst.DOC_SERVICE_APPLICATION;
        public int RegionId { get; set; }
        public string Organization { get; set; }
        public string Region { get; set; }
        public int? DistrictId { get; set; }
        public string? District { get; set; }
        public  bool ToRegionalOffice { get; set; }
        #region Actions
        public bool CanEdit { get; set; }
        public bool CanAccept { get; set; }
        public bool CanSend { get; set; }
        public bool CanDelete { get; set; }
        public bool CanCancel { get; set; }
        public bool CanReject { get; set; }
        public bool CanReceived { get; set; }
        public bool CanCreateContract { get; set; }
        #endregion

        public new List<SrvApplicationGroupDto> Groups { get; set; } = new();
    }
}