using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public class StaffingIndicatorValueDto : StaffingIndicatorValueDlDto, ILinkToEntity<StaffingIndicatorValue>
    {
        public decimal? Quantity { get; set; } = 0;
        public decimal? TotalSum { get; set; } = 0;
        public string StaffingIndicatorName { get; set; }
        public string StaffingIndicatorCode { get; set; }
        public int Order { get; set; }
    }
}
