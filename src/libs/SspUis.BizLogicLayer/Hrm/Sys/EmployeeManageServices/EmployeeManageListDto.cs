using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;
using System.ComponentModel.DataAnnotations.Schema;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.EmployeeManageServices
{
    public class EmployeeManageListDto : ILinkToEntity<EmployeeManage>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public long DocId { get; set; }
        public DateOnly? StartOn { get; set; }
        public DateOnly? EndOn { get; set; }
        public int EmpAppointOrderTypeId { get; set; }
        public int EmploymentTypeId { get; set; }
        public int WorkScheduleId { get; set; }
        public string EmpAppointOrderType { get; set; }
        public string EmploymentType { get; set; }
        public string WorkSchedule { get; set; }
        public decimal EmploymentRate { get; set; }
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public int PositionClassificationId { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Employee { get; set; }
        public string EmployeePinfl { get; set; }
        public DateOnly? EmployeeBirthOn { get; set; }
        public string EmployeePhoneNumber { get; set; }
        public int OrganizationId { get; set; }
        public string Organization { get; set; }
        public int GenderId { get; set; }
        public string? Gender { get; set; }
    }
}
