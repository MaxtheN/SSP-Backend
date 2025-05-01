using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public class UpdateStatusAppointEmployeeDlDto : EntityDto<UpdateStatusAppointEmployeeDlDto, AppointEmployee>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }
        public string Message { get; set; } = null!;
        [LocalizedRequired]
        public int StatusId { get; set; }
    }
}
