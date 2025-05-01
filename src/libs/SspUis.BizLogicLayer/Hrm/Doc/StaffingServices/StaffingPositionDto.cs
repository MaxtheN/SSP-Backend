using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public class StaffingPositionDto : StaffingPositionDlDto, ILinkToEntity<StaffingPosition>
    {
        public string DepartmentName { get; set; } = null!;
        public string DepartmentCode { get; set; } = null!;
        public string PositionName { get; set; } = null!;
        public string PositionOrderCode { get; set; } = null!;
        public string PositionTypeName { get; set; } = null!;
        public string PositionCategoryName { get; set; } = null!;
        public string QualificationCategoryName { get; set; } = null!;
        public string TariffScaleName { get; set; } = null!;
        public string RankName { get; set; } = null!;
        public string PositionClassification { get; set; } = null!;

        public string TariffScaleTypeName { get; set; } = null!;
        public string StaffingCalcKindNames { get; set; } = null!;
        public DateTime OwnerDocDate { get; set; }
        public decimal? FixedValue { get; set; }
        public decimal? CalcKindsCalcSum { get; set; }
        public string PositionPeriod { get; set; } = null!;
        public int PositionPeriodMonthCount { get; set; }
        public int FunctionalItemOfExpenseId { get; set; }

        new public List<StaffingCalcKindDto> CalcKinds { get; set; } = new();

    }
}
