using WEBASE.EF;
using WEBASE.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WEBASE.Attributes;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public class StaffingTemplateTableDlDto : EntityDto<StaffingTemplateTableDlDto, StaffingTemplateTable>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int PositionId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int TariffScaleTypeId { get; set; }
        public int? TariffScaleId { get; set; }
        public int? TariffScaleTableId { get; set; }
        public string? RankCode { get; set; }
        public string? RankName { get; set; }
        public decimal? Quantity { get; set; }
    }
}
