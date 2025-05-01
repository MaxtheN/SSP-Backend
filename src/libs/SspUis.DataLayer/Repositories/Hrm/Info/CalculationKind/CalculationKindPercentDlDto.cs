using SspUis.DataLayer.EfClasses.Hrm;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class CalculationKindPercentDlDto : EntityDto<CalculationKindPercentDlDto, CalculationKindPercent>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        public DateOnly DateOn { get; set; }
        [LocalizedRequired]
        public decimal PercentRate { get; set; }
        public string Details { get; set; }
        public int? LimitOperTypeId { get; set; }
        public decimal? Amount { get; set; }
    }
}
