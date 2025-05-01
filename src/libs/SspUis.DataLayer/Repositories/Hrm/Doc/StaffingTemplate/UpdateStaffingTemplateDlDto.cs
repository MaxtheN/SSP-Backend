using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public class UpdateStaffingTemplateDlDto : StaffingTemplateDlDto<UpdateStaffingTemplateDlDto>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }
    }
}
