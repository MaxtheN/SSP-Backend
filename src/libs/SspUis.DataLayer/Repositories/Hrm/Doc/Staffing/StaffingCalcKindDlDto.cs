using AutoMapper;
using GenericServices;
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
    public class StaffingCalcKindDlDto : EntityDto<StaffingCalcKindDlDto, StaffingCalcKind>, IHaveIdProp<long>, ILinkToEntity<StaffingCalcKind>
    {
        public long Id { get; set; }
        public int? CalculationKindId { get; set; }
        public decimal? CalcCoef { get; set; }
        public decimal? CalcSum { get; set; }
        
    }
}
