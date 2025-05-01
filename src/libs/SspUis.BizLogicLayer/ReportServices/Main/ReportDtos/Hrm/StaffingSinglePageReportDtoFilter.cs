using Newtonsoft.Json;
using SspUis.Integration.BankCredit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class StaffingSinglePageReportDtoFilter : DocumentSortFilterOptions
    {
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Employees { get; set; }
        public int? OrganizationId { get; set; }
        public bool Acting { get; set; }
        public bool Interm { get; set; }
        public bool IsProbation { get; set; }
        public bool ByQuantity { get; set; }
        public bool ByQuantityForNow { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateTime? StartCreatedAt { get; set; }
        public DateTime? EndCreatedAt { get; set; }
    }
}
