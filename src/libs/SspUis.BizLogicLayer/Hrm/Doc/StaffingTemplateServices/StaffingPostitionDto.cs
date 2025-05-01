namespace SspUis.BizLayer.Hrm.StaffingTemplateServices
{
    public class StaffingPostitionDto
    {
        public int PositionId { get; set; }
        public string Position{ get; set; } = null!;
        public int? TariffScaleTypeId { get; set; }
        public int? StaffTypeBasicTariffId { get; set; }
        public string TariffScaleType{ get; set; } = null!;
        public string StaffTypeBasicTariff{ get; set; } = null!; 
    }
}
