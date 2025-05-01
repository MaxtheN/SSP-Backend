using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices.Main
{
    public class BusinessActivityTypeReportDto
    {
        public string BankName { get; set; }
        public int? BankCodeId { get; set; }
        public BusinessActivityReport BusinessActivity { get; set; }
        public BusinessActivityReport BusinessType1 { get; set; }
        public BusinessActivityReport BusinessType2 { get; set; }
        public BusinessActivityReport BusinessType3 { get; set; }

    }
    public class BusinessActivityReport
    {
        public int UserPrivilegeCount { get; set; }
        public int CreatedVacanciesCount { get; set; }
        public decimal FinancialHelpAmount { get; set; }
        public decimal ApprovedFinancialHelpAmount { get; set; }
    }
    public class BusinessActivityTypeReportByRegion
    {
        public int? RegionId { get; set; }
        public string RegionName { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public BusinessActivityReport BusinessActivity { get; set; }
        public BusinessActivityReport BusinessType1 { get; set; }
        public BusinessActivityReport BusinessType2 { get; set; }
        public BusinessActivityReport BusinessType3 { get; set; }
    }
}