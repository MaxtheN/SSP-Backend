using SspUis.DataLayer.EfClasses.Hrm;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class CalculationKindUsedTableDlDto : EntityDto<CalculationKindUsedTableDlDto, CalculationKindUsedTable>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        public DateOnly StartOn { get; set; }
        public DateOnly? EndOn { get; set; }
        [LocalizedRequired]
        public bool CalcFromInSum { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int FormedCalculationKindId { get; set; }
        public int? MinimumValueTypeId { get; set; }
        public decimal? QuantityOfMinimumValue { get; set; }
    }
}
