using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.Edoc.AppointmentOrderService
{
    public class AppointmentOrderDto
    {
        public int OrganizationId { get; set; }
        public int EmployeeId { get; set; }
        public int AppointmentTypeId { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public int StatusId { get; set; }
        public decimal EmployeeRate { get; set; }
        [LocalizedStringLength(20)]
        public string DocNumber { get; set; }
        public DateTime DocOn { get; set; }
        public DateTime StartOn { get; set; }
        public DateTime? EndateOn { get; set; }
    }
}
