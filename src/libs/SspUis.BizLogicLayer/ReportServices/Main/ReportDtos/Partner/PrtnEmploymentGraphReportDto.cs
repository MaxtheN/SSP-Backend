using Newtonsoft.Json;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class PrtnEmploymentGraphPageOption : PageOptions
    {
        public int? YearIn { get; set; }
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
        public long? MfyId { get; set; }
        public long? ContractorId { get; set; }
        public string? ContractorInn { get; set; }
    }
    public class PrtnEmploymentGraphReportDto
    {
        public long TotalCount { get; set; }
        public List<PrtnEmploymentGraphReportRowsDto> Rows { get; set; }
        public Dictionary<int, string> Columns { get; set; }
        public long TotalEmployeesCountUntilFounded { get; set; }
        public (long TotalGraphYears, long TotalTaxYears, long TotalDifferenceYears) TotalYears { get; set; }
        public (long TotalPlanGraph, long TotalDifferenceGraphAndReport, long TotalDifference) Total { get; set; }
        public Dictionary<int, (long TotalPlanGraphCount, long TotalDifferenceGraphAndReportCount, long TotalDifferenceCount)> TotalMonthly { get; set; } = new();
    }
    public class PrtnEmploymentGraphReportRowsDto
    {
        [JsonIgnore]
        public IEnumerable<PrtnEmploymentGraphReportItemsDto> PlanGrap { get; set; }
        [JsonIgnore]
        public IEnumerable<PrtnEmploymentGraphReportItemsDto> ByReport { get; set; }
        [JsonIgnore]
        public long GraphYears { get; set; }
        [JsonIgnore]
        public long TaxYears { get; set; }
        public int? RegionId { get; set; }
        public string RegionOrderCode { get; set; }
        public string Region { get; set; }
        public int? DistrictId { get; set; }
        public string District { get; set; }
        public long? MfyId { get; set; }
        public string Mfy { get; set; }
        public long? ContractorId { get; set; }
        public string ContractorName { get; set; }
        public string ContractorInn { get; set; }
        public long EmployeesCountUntilFounded { get; set; }
        public (long GraphYearsCount, long TaxYearsCount, long DifferenceYearsCount) Years { get; set; }
        public (long TotalPlanGraph, long TotalDifferenceGraphAndReport, long TotalDifference) RowsTotal { get; set; }
        public Dictionary<int, (long PlanGraphCount, long DifferenceGraphAndReportCount, long DifferenceCount)> RowsMonthly { get; set; } = new();
    }
    public class PrtnEmploymentGraphReportItemsDto
    {
        public int YearIn { get; set; }
        public int MonthIn { get; set; }
        public long ParamSumm { get; set; }
    }
    public static class CommonConst
    {
        /// <summary>
        /// JAMI 3 YILLIK HISOBOT UCHUN KERAK EKAN
        /// </summary>
        public const int _2023 = 2023;
        public const int _2024 = 2024;
        public const int _2025 = 2025;
        public const int _2026 = 2026;

        /// <summary>
        /// SSP AVTOMATLASHTIRISHDAN OLDIGI 1 OY LIK HISOBOT UCHUN
        /// </summary>
        public const int MAY = 5;

        /// <summary>
        /// SSP AVTOMATLASHTIRILGANDAN KEYINGGI OYDAN HISOBOT KETISHI UCHUN
        /// </summary>
        public const int IYUN = 6;

        /// <summary>
        /// 12 OY UCHUN MO'LJALLANGAN INDEX
        /// </summary>
        public const int _1 = 1;
        public const int _12 = 12;
    }
}