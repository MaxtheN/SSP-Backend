using System;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class StaffCountByGenderDto
    {
        public int? RegionId { get; set; }
        public string RegionOrderCode { get; set; }
        public string Region { get; set; }

        public int? DistrictId { get; set; }
        public string District { get; set; }

        public int? OrganizationId { get; set; }
        public string Organization { get; set; }
        public string OrganizationInn { get; set; }
        public string OrganizationOrderCode { get; set; }

        public int? DepartmentId { get; set; }
        public string DepartmentOrderCode { get; set; }
        public string Department { get; set; }

        public int? PositionId { get; set; }
        public string PositionOrderCode { get; set; }
        public string Position { get; set; }

        public DateOnly? StartOn { get; set; }
        public DateOnly? EndOn { get; set; }

        public int? WorkScheduleId { get; set; }
        public string WorkSchedule { get; set; }
        public int? EmpAppointOrderTypeId { get; set; }
        public string EmpAppointOrderType { get; set; }
        public decimal? EmploymentRate { get; set; }
        public int? EmploymentTypeId { get; set; }
        public string EmploymentType { get; set; }
        public long? EmployeeId { get; set; }
        public long? DocId { get; set; }
        public string Employee { get; set; }
        public DateOnly? EmployeeBirthDate { get; set; }
        public string EmployeeGender { get; set; }
        public string EmployeePinfl { get; set; }
        public string EmployeeNationality { get; set; }
        public string EmployeePhoneNumber { get; set; }

        public decimal? TotalStaffingRate { get; set; }
        public decimal? TotalEmployeeManageRate { get; set; }
        public decimal? TotalCount { get; set; }

        public long? TotalEmployee { get; set; }
        public long? TotalEmployeeMen { get; set; }
        public long? TotalEmployeeWomen { get; set; }

        public IEnumerable<PositionCategoryDtoGendre> PositionCategorys { get; set; }

    }
    public class PositionCategoryDtoGendre 
    {
        public int? PositionCategoryId { get; set; }
        public string PositionCategory { get; set; }
        public int Men { get; set; }
        public int Women { get; set; }
        public int Count { get; set; } 
    }
}
