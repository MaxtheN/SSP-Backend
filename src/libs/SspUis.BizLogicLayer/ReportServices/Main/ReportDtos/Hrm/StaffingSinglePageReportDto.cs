using Newtonsoft.Json;
using SspUis.Integration.BankCredit.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class StaffingSinglePageReportDto
    {
        public int DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityForNow { get; set; }
        public int? OrganizationId { get; set; }
        public List<EmployeeManageTable> EmployeeManageTables { get; set; }
    }

    public class EmployeeManageTable
    {
        public string Pinfl { get; set; }
        public string Employees { get; set; }
        public int? EmployeeId { get; set; }
        public int? GenderId { get; set; }
        public long? EmployeeManageId { get; set; }
        public long? EmployeeManageDocId { get; set; }
        public decimal? EmployeeRate { get; set; }
        public List<AppointEmployeeTables> AppointEmployees { get; set; }
    }
    public class AppointEmployeeTables
    {
        public string DocNumber { get; set; }
        public DateOnly? DocOn { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? StatusId { get; set; }
        public bool Acting { get; set; }
        public bool Interm { get; set; }
        public bool IsProbation { get; set; }
    }
}
