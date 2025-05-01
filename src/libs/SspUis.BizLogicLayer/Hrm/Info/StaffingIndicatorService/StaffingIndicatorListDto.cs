using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using System;

namespace SspUis.BizLogicLayer.Hrm.StaffingIndicatorServices
{
    public class StaffingIndicatorListDto :  ILinkToEntity<StaffingIndicator>
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateOnly StartOn { get; set; }
        public DateOnly? EndOn { get; set; }
        public string State{ get; set; } = null!;
        public int StateId { get; set; }
    }
}
