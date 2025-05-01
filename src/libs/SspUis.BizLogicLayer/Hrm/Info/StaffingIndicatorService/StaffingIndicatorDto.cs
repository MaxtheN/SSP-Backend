using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Interfaces;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Hrm.StaffingIndicatorServices
{
    public class StaffingIndicatorDto : UpdateStaffingIndicatorDlDto, ILinkToEntity<StaffingIndicator>, IInfoHl
    {
        public string State { get; set; } = null!;
        public new List<StaffingIndicatorTranslateDto> Translates { get; set; } = new();
    }
}
