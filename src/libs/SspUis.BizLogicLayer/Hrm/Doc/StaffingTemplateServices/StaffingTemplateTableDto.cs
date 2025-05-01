using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLayer.Hrm.StaffingTemplateServices
{
    public class StaffingTemplateTableDto : StaffingTemplateTableDlDto, ILinkToEntity<StaffingTemplateTable>
    {
        public string Position { get; set; } = null!;
        public string TariffScaleType { get; set; } = null!;
        public string TariffScale { get; set; } = null!;
        public decimal? TariffScaleCoef { get; set; }
        public string? TariffScaleTable { get; set; }
    }
}
