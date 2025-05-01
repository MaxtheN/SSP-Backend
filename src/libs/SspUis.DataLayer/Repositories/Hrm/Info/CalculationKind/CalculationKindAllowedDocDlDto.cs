using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class CalculationKindAllowedDocDlDto : EntityDto<CalculationKindAllowedDocDlDto, CalculationKindAllowedDoc>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        public DateOnly DateOn { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int TableId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int StateId { get; set; }
    }
}
