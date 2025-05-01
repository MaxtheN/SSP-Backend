using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class ServicePriceListDto : ILinkToEntity<ServicePrice>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public string DocNumber { get; set; }
        public DateOnly DocOn { get; set; }
        public string Details { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int OrganizationId { get; set; }
        public string Organization { get; set; }
        public DateTime CreatedAt { get; set; }
        #region Actions
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanAccept { get; set; }
        public bool CanCancel { get; set; }
        #endregion
        public List<ServicePriceGroupListDto> Groups { get; set; }
    }
}
