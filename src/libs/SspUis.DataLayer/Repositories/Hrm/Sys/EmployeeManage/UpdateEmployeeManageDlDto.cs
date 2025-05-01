using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateEmployeeManageDlDto : EmployeeManageDlDto<UpdateEmployeeManageDlDto>, IHaveIdProp<long>
    {
        //[LocalizedRequired]
        //[LocalizedRange(1, long.MaxValue)]
        //public long Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
