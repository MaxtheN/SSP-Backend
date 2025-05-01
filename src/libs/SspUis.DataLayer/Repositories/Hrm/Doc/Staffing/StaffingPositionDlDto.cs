using AutoMapper;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public class StaffingPositionDlDto : EntityDto<StaffingPositionDlDto, StaffingPosition>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public int? OrderNumber { get; set; }
        public int? PositionTypeId { get; set; }
        public int? PositionCategoryId { get; set; }
        public int? QualificationCategoryId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int TariffScaleTypeId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int PositionClassificationId { get; set; }
        public int? TariffScaleId { get; set; }
        //[LocalizedRequired]
        //[LocalizedRange(1, int.MaxValue)]
        public int PositionPeriodId { get; set; }
        public int? RankId { get; set; }
        [LocalizedRequired]
        public decimal Quantity { get; set; }
        public decimal? TotalSum { get; set; }
        [LocalizedStringLength(4)]
        public string? RankCode { get; set; }
        public decimal? RankCoef { get; set; }
        public decimal? CorrCoef { get; set; }
        public decimal? Fot { get; set; }
        public int ForMonth { get; set; }
        [LocalizedRequired]
        public decimal Salary { get; set; }
        public List<StaffingCalcKindDlDto> CalcKinds { get; set; } = new List<StaffingCalcKindDlDto>();

        protected override Action<IMappingExpression<StaffingPositionDlDto, StaffingPosition>> AlterMapping => cfg => cfg
            .ForMember(x => x.CalcKinds, x => x.Ignore());
    }
}
